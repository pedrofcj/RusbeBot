using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Discord;
using Discord.Commands;
using RusbeBot.Helpers;
using Sentry;

namespace RusbeBot.Attributes;

public class CommandValidation : PreconditionAttribute
{
    private readonly bool _isChannelSensitive;
    private readonly bool _isRoleSensitive;

    public CommandValidation(bool isChannelSensitive, bool isRoleSensitive)
    {
        _isChannelSensitive = isChannelSensitive;
        _isRoleSensitive = isRoleSensitive;
    }

    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        if (!(context.User is IGuildUser user))
            return PreconditionResult.FromError("Este comando só pode ser utilizado em um servidor.");

        // override if message was sent by bot owner
        var application = await context.Client.GetApplicationInfoAsync();
        if (user.Id == application.Owner.Id)
            return PreconditionResult.FromSuccess();

        // override de roles para o dono do server
        if (user.Id == user.Guild.OwnerId)
            return PreconditionResult.FromSuccess();

        if (await ValidateModeradorAsync(context, services, user))
            return PreconditionResult.FromSuccess();

        var chanelPreconditionResult = await ValidateChannelAsync(context, command, services);
        var rolePreconditionResult = await ValidateRoleAsync(context, command, services, user);

        if (chanelPreconditionResult.IsSuccess && rolePreconditionResult.IsSuccess)
            return PreconditionResult.FromSuccess();

        var errorMessage = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(chanelPreconditionResult.ErrorReason))
            errorMessage.AppendLine(chanelPreconditionResult.ErrorReason);
        if (!string.IsNullOrWhiteSpace(rolePreconditionResult.ErrorReason))
            errorMessage.AppendLine(rolePreconditionResult.ErrorReason);

        await context.Channel.SendMessageAsync(errorMessage.ToString());

        var logMessage = $"Erro ao executar o comando '!{command.Name}'. {Environment.NewLine}" +
                           $"Requisitado por {context.User.Username}#{context.User.Discriminator} em {context.Guild.Name}/{context.Channel.Name} ({context.Guild.Id}/{context.Channel.Id}) {Environment.NewLine}" +
                           $"Mensagem de erro: {errorMessage} {Environment.NewLine}" +
                           $"Raw message: {context.Message.Content}";

        SentryHelper.Log(logMessage, context, SentryLevel.Warning, new List<KeyValuePair<string, string>>
        {
            new("Tipo", "CommandValidation"),
            new("Comando", command.Name)
        });

        return PreconditionResult.FromError(errorMessage.ToString());
    }

    private async Task<PreconditionResult> ValidateChannelAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        // busca no DI o serviço de configuração
        if (services.GetService(typeof(IAllowedChannelsConfigService)) is not IAllowedChannelsConfigService allowedConfig)
            return PreconditionResult.FromError(
                "Houve um erro ao tentar buscar as configurações para este comando. Entre em contato com o dono do servidor para reportar este erro.");

        // busca as configs para este command
        var commandConfigs = await allowedConfig.GetAllowedChannelsByCommandAndGuild(command.Name, context.Guild.Id.ToString());

        // nenhuma config encontrada, deixa prosseguir se o comando nao tiver marcado como sensitive
        if (!commandConfigs.Any() && !_isChannelSensitive)
            return PreconditionResult.FromSuccess();

        // busca id do canal
        var channelId = context.Channel.Id;

        // monta a lista de canais autorizados
        var authorizedChannels = commandConfigs.Select(model => Convert.ToUInt64(model.ChannelId)).ToList();

        // retorna o resultado
        return authorizedChannels.Any(d => d == channelId) ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(ErrorMessage ?? "Este comando não pode ser utilizado nesta sala. Entre em contato com o dono do servidor para reportar este erro.");
    }

    private async Task<PreconditionResult> ValidateRoleAsync(ICommandContext context, CommandInfo command, IServiceProvider services, IGuildUser user)
    {
        // busca no DI o serviço de configuração
        if (services.GetService(typeof(IAllowedRolesConfigService)) is not IAllowedRolesConfigService allowedConfig)
            return PreconditionResult.FromError(
                "Houve um erro ao tentar buscar as configurações para este comando. Entre em contato com o dono do servidor para reportar este erro.");

        // busca as configs para este command
        var commandConfigs = await allowedConfig.GetAllowedRolesByCommandAndGuild(command.Name, context.Guild.Id.ToString());

        // nenhuma config encontrada, deixa prosseguir se o comando nao tiver marcado como sensitive
        if (!commandConfigs.Any() && !_isRoleSensitive)
            return PreconditionResult.FromSuccess();

        // busca as roles do user
        var userRoles = user.RoleIds.ToList();

        // monta a lista de roles autorizadas
        var authorizedRoles = commandConfigs.Select(model => Convert.ToUInt64(model.RoleId)).ToList();

        // retorna o resultado
        return authorizedRoles.Intersect(userRoles).Any() ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(ErrorMessage ?? "Você não tem permissão para executar esse comando. Entre em contato com o dono do servidor para reportar este erro.");
    }

    private async Task<bool> ValidateModeradorAsync(ICommandContext context, IServiceProvider services, IGuildUser user)
    {
        // busca no DI o serviço de configuração
        if (services.GetService(typeof(IModeradorService)) is not IModeradorService allowedConfig)
        {
            SentryHelper.Log("Houve um erro ao tentar buscar as configurações para este comando. Entre em contato com o dono do servidor para reportar este erro.", context, SentryLevel.Error);
            return false;
        }

        // busca se o user é moderador
        var moderador = await allowedConfig.GetModeradorByUserIdAsync(user.Id.ToString());
        return moderador != null;
    }

}
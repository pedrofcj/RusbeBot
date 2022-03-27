using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Discord;
using Discord.Commands;

namespace TheLostBot.Attributes
{
    public class AllowedChannels : PreconditionAttribute
    {

        private readonly bool _isSensitive;
        public AllowedChannels(bool isSensitive)
        {
            _isSensitive = isSensitive;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (!(context.User is IGuildUser user))
                return PreconditionResult.FromError("Este comando só pode ser utilizado em um servidor.");

            // override de roles para o dono do server
            if (user.Id == user.Guild.OwnerId)
                return PreconditionResult.FromSuccess();

            // busca no DI o serviço de configuração
            if (services.GetService(typeof(IAllowedChannelsConfigService)) is not IAllowedChannelsConfigService allowedConfig)
                return PreconditionResult.FromError(
                    "Houce um erro ao tentar buscar as configurações para este comando. Entre em contato com o dono do servidor para reportar este erro.");

            // busca as configs para este command
            var commandConfigs = await allowedConfig.GetAllowedChannelsByCommandAndGuild(command.Name, context.Guild.Id.ToString());

            // nenhuma config encontrada, deixa prosseguir se o comando nao tiver marcado como sensitive
            if (!commandConfigs.Any() && !_isSensitive)
                return PreconditionResult.FromSuccess();

            // busca id do canal
            var channelId = context.Channel.Id;

            // monta a lista de canais autorizados
            var authorizedChannels = commandConfigs.Select(model => Convert.ToUInt64(model.ChannelId)).ToList();
            
            // retorna o resultado
            return authorizedChannels.Any(d => d == channelId) ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(ErrorMessage ?? "Este comando não pode ser utilizado nesta sala.");
        }

    }
}
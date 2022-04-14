using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Discord;
using Discord.Commands;
using RusbeBot.Attributes;

namespace RusbeBot.Modules.Config;

[CommandValidation(false, true)]
[RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]
public class ConfigChannelsModule : ConfigModule
{
    public ConfigChannelsModule(IAllowedChannelsConfigService allowedChannelsConfigService, IAllowedRolesConfigService allowedRolesConfigService) : base(allowedChannelsConfigService, allowedRolesConfigService)
    {
    }

    [Command("channel add")]

    public async Task AddConfigChannelAsync([Remainder] string input)
    {
        try
        {
            var command = input.Split(' ')[0];

            await AllowedChannelsConfigService.InsertAsync(new AllowedChannelsConfigModel
            {
                ChannelId = Context.Channel.Id.ToString(),
                CommandName = command,
                GuildId = Context.Guild.Id.ToString()
            });

            await ReplyAsync("Este canal foi adicionado com sucesso");
        }
        catch (Exception e)
        {
            await ReplyAsync("Houve um erro ao tentar adicionar as roles");
            Console.WriteLine(e);
            throw;
        }
    }

    [Command("channel remove")]
    public async Task RemoveConfigChannelAsync([Remainder] string input)
    {
        try
        {
            var command = input.Split(' ')[0];

            var allConfigChannels = await AllowedChannelsConfigService.GetAllowedChannelsByCommandAndGuild(command, Context.Guild.Id.ToString());

            var configChannel = allConfigChannels.SingleOrDefault(model => model.ChannelId == Context.Channel.Id.ToString());

            if (configChannel != null)
            {
                await AllowedChannelsConfigService.DeleteAsync(configChannel.Id);
            }

            await ReplyAsync("Canal removidas com sucesso");
        }
        catch (Exception e)
        {
            await ReplyAsync("Houve um erro ao tentar remover o canal");
            Console.WriteLine(e);
            throw;
        }
    }

    [Command("channel list")]
    public async Task ListConfigChannelAsync([Remainder] string input)
    {
        try
        {
            var command = input.Split(' ')[0];

            var allAllowedChannels = await AllowedChannelsConfigService.GetAllowedChannelsByCommandAndGuild(command, Context.Guild.Id.ToString());

            var response = new StringBuilder();
            response.AppendLine("Canais onde este comando podem ser executado: ");

            foreach (var allowedChannel in allAllowedChannels)
            {
                response.AppendLine(MentionUtils.MentionChannel(Convert.ToUInt64(allowedChannel.ChannelId)));
            }

            await ReplyAsync(response.ToString());
        }
        catch (Exception e)
        {
            await ReplyAsync("Houve um erro ao tentar listar os canais onde este comando pode ser executado");
            Console.WriteLine(e);
            throw;
        }
    }

}
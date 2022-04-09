using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheLostBot.Attributes;
using TheLostBot.Extensions;

namespace TheLostBot.Modules;

[Name("Moderator")]
[RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]
[CommandValidation(false, true)]
public class ModeratorModule : ModuleBase<SocketCommandContext>
{

    #region Taxar

    [Command("taxar")]
    public async Task Taxar([Remainder] SocketGuildUser user)
    {
        TaxadosExtension.IdsTaxados.Add(user.Id);
        await ReplyAsync($"Você está sendo taxado {user.Mention}");
        await Context.Message.DeleteAsync();
    }

    [Command("liberar")]
    public async Task Liberar([Remainder] SocketGuildUser user)
    {
        TaxadosExtension.IdsTaxados.Remove(user.Id);
        await ReplyAsync($"Você ta liberado {user.Mention}");
        await Context.Message.DeleteAsync();
    }

    #endregion

    #region Clear

    [Command("clear")]
    public async Task ClearAsync(int quantidade)
    {
        var messages = await Context.Channel.GetMessagesAsync(quantidade + 1).Flatten().ToListAsync();

        if (!(Context.Channel is ITextChannel channel)) return;

        await channel.DeleteMessagesAsync(messages);

        const int delay = 5000;
        var m = await ReplyAsync($"Mensagens apagadas. _Esta mensagem será excluída em {delay / 1000} segundos._");
        await Task.Delay(delay);
        await m.DeleteAsync();

    }

    #endregion

    #region Disconnect

    [Command("dc")]
    public async Task DcAsync([Remainder] SocketGuildUser user)
    {
        await Context.Message.DeleteAsync();
        await user.ModifyAsync(properties => properties.Channel = null);
    }

    #endregion

    #region Mute/Unmute

    [Command("mute")]
    public async Task MuteAsync([Remainder] SocketGuildUser user)
    {
        await Context.Message.DeleteAsync();
        await user.ModifyAsync(properties => properties.Mute = true);
    }

    [Command("unmute")]
    public async Task UnmuteAsync([Remainder] SocketGuildUser user)
    {
        await Context.Message.DeleteAsync();
        await user.ModifyAsync(properties => properties.Mute = false);
    }

    #endregion

}
using System;
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
    public async Task TaxarAsync(SocketGuildUser user)
    {
        
        TaxadosExtension.Add(Context.Guild.Id, user.Id);
        await ReplyAsync($"Você está sendo taxado {user.Mention}");
        await Context.Message.DeleteAsync();
    }

    [Command("liberar")]
    public async Task LiberarAsync(SocketGuildUser user)
    {
        TaxadosExtension.Remove(Context.Guild.Id, user.Id);
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

    #region Timeout

    [Command("to-s")]
    public async Task TimeoutSegundosAsync(SocketGuildUser user, int segundos)
    {
        await TimeoutAsync(user, TimeSpan.FromSeconds(segundos));
        await Context.Message.DeleteAsync();
    }

    [Command("to-m")]
    public async Task TimeoutMinutosAsync(SocketGuildUser user, int minutos)
    {
        await TimeoutAsync(user, TimeSpan.FromMinutes(minutos));
        await Context.Message.DeleteAsync();
    }

    [Command("to-h")]
    public async Task TimeoutHorasAsync(SocketGuildUser user, int horas)
    {
        await TimeoutAsync(user, TimeSpan.FromHours(horas));
        await Context.Message.DeleteAsync();
    }

    [Command("to-d")]
    public async Task TimeoutDiasAsync(SocketGuildUser user, int dias)
    {
        await TimeoutAsync(user, TimeSpan.FromDays(dias));
        await Context.Message.DeleteAsync();
    }

    private async Task TimeoutAsync(SocketGuildUser user, TimeSpan tempo)
    {
        await user.SetTimeOutAsync(tempo);
    }

    #endregion

}
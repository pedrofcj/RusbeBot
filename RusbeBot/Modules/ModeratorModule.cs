using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RusbeBot.Attributes;
using RusbeBot.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RusbeBot.Modules;

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

    [Command("tot")]
    public async Task TimeoutAsync(SocketGuildUser user, [Remainder] string tempoText)
    {
        // check if user is admin
        if (user.GuildPermissions.Administrator)
        {
            await ReplyAsync($"{user.Mention} é um administrador e não pode tomar timeout.");
            return;
        }

        var time = GetTimeSpanFromText(tempoText);
        await user.SetTimeOutAsync(time);
        await Context.Message.DeleteAsync();
    }

    private TimeSpan GetTimeSpanFromText(string text)
    {
        // extract just numbers from text
        var numbers = new string(text.Where(char.IsDigit).ToArray());

        // extract unit from text
        return text.ToLower().Last() switch
        {
            's' => TimeSpan.FromSeconds(int.Parse(numbers)),
            'm' => TimeSpan.FromMinutes(int.Parse(numbers)),
            'h' => TimeSpan.FromHours(int.Parse(numbers)),
            'd' => TimeSpan.FromDays(int.Parse(numbers)),
            _ => TimeSpan.FromSeconds(int.Parse(numbers))
        };
    }

    #endregion

}
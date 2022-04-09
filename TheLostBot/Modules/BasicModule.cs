﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheLostBot.Attributes;

namespace TheLostBot.Modules;

[CommandValidation(false, true)]
[RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]

public class BasicModule : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    public async Task Say([Remainder] string text)
    {
        await Context.Message.DeleteAsync();
        await ReplyAsync(text);
    }

    [Command("msg")]
    public async Task Message(SocketGuildUser user, [Remainder] string text)
    {
        await Context.Message.DeleteAsync();
        await user.SendMessageAsync(text);
    }
}
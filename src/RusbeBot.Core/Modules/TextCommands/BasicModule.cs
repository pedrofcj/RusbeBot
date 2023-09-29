using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RusbeBot.Core.Attributes;

namespace RusbeBot.Core.Modules.TextCommands;

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
    
    [Command("kickFiveM")]
    public async Task KickFiveM(int userId)
    {
        var url = $"http://localhost:30120/sets/players/{userId}";
        
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            await ReplyAsync($"Usuário {userId} kickado do FiveM");
        }
        else
        {
            await ReplyAsync($"Usuário {userId} não encontrado no FiveM");
        }
    }
    
}
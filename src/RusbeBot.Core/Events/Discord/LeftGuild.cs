using Discord.WebSocket;

namespace RusbeBot.Core.Events.Discord;

public class LeftGuild
{
    public async Task HandleEventAsync(SocketGuild arg)
    {
       
        await Task.CompletedTask;
    }
}
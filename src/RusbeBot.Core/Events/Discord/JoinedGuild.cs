using Discord.WebSocket;
using RusbeBot.Core.Helpers;

namespace RusbeBot.Core.Events.Discord;

public class JoinedGuild
{
    public async Task HandleEventAsync(SocketGuild arg)
    {
        SentryHelper.Log($"Joined guild {arg.Name} ({arg.Id})", null);
        await Task.CompletedTask;
    }
}
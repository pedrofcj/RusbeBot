using Discord.WebSocket;
using RusbeBot.Core.Helpers;

namespace RusbeBot.Core.Events.Discord;

public class LeftGuild
{
    public async Task HandleEventAsync(SocketGuild arg)
    {
        SentryHelper.Log($"Left guild {arg.Name} ({arg.Id})", null);
        await Task.CompletedTask;
    }
}
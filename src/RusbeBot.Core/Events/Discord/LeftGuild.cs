using Discord.WebSocket;
using Sentry;

namespace RusbeBot.Core.Events.Discord;

public class LeftGuild
{
    public async Task HandleEventAsync(SocketGuild arg)
    {
        SentrySdk.CaptureMessage($"Saiu no server {arg.Name} ({arg.Id})", scope =>
        {
            scope.SetTags(new List<KeyValuePair<string, string>>
            {
                new("Tipo", "Saiu")
            });
        });
        await Task.CompletedTask;
    }
}
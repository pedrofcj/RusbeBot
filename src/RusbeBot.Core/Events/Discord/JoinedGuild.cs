using Discord.WebSocket;
using Sentry;

namespace RusbeBot.Core.Events.Discord;

public class JoinedGuild
{
    public async Task HandleEventAsync(SocketGuild arg)
    {
        SentrySdk.CaptureMessage($"Entrou no server {arg.Name} ({arg.Id})", scope =>
        {
            scope.SetTags(new List<KeyValuePair<string, string>>
            {
                new("Tipo", "Entrou")
            });
        });
        await Task.CompletedTask;
    }
}
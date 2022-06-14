using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Helpers;
using Sentry;

namespace RusbeBot.Core.Events.Discord;

public class Ready
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly IConfigurationRoot _config;

    public Ready(DiscordSocketClient client, InteractionService interactionService, IConfigurationRoot config)
    {
        _client = client;
        _interactionService = interactionService;
        _config = config;
    }

    public async Task HandleEventAsync()
    {
        Console.WriteLine($"{_client.CurrentUser.Username} is online!");

        await RegisterCommands();
    }
    
    private async Task RegisterCommands()
    {
        var isDebug = _config["isDebug"];

        if (string.IsNullOrEmpty(isDebug))
        {
            try
            {
                await _interactionService.RegisterCommandsGloballyAsync();
                return;
            }
            catch (Exception e)
            {
                SentrySdk.CaptureException(e);
                Console.WriteLine(e);
            }
            
        }
        
        var guild = _config["debugGuildId"];
        
        await _interactionService.RegisterCommandsToGuildAsync(Convert.ToUInt64(guild));
        
    }
}
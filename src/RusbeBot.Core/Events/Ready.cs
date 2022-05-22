using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Services;

namespace RusbeBot.Core.Events;

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
            await _interactionService.RegisterCommandsGloballyAsync();
            return;
        }
        
        var guild = _config["guildId"];
        
        await _interactionService.RegisterCommandsToGuildAsync(Convert.ToUInt64(guild));
        
    }
}
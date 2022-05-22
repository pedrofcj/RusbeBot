using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Events;
using RusbeBot.Core.Helpers;

namespace RusbeBot;

public class DiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly Ready _ready;
    private readonly MessageReceived _messageReceived;
    private readonly InteractionCreated _interactionCreated;
    private readonly IConfigurationRoot _config;

    public DiscordBot(DiscordSocketClient client, MessageReceived messageReceived, InteractionCreated interactionCreated, IConfigurationRoot config, Ready ready)
    {
        _client = client;
        _messageReceived = messageReceived;
        _interactionCreated = interactionCreated;
        _config = config;
        _ready = ready;
    }

    public async Task RunAsync()
    {
        var discordToken = _config["tokens:discord"];     // Get the discord token from the config file
        if (string.IsNullOrWhiteSpace(discordToken))
            throw new Exception("Por favor informe o token do bot no arquivo `_config.yml`.");
        
        await _client.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
        await _client.SetGameAsync("👀");
        await _client.StartAsync();                                // Connect to the websocket
        
        _client.Ready += _ready.HandleEventAsync;
        _client.MessageReceived += _messageReceived.HandleEventAsync;
        _client.InteractionCreated += _interactionCreated.HandleEventAsync;

        await Task.Delay(-1);
    }
}
using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Events;
using RusbeBot.Core.Events.Discord;
using RusbeBot.Core.Helpers;

namespace RusbeBot;

public class DiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly Ready _ready;
    private readonly MessageReceived _messageReceived;
    private readonly InteractionCreated _interactionCreated;
    private readonly IConfigurationRoot _config;
    private readonly JoinedGuild _joinedGuild;
    private readonly LeftGuild _leftGuild;

    public DiscordBot(DiscordSocketClient client, MessageReceived messageReceived, InteractionCreated interactionCreated, IConfigurationRoot config, Ready ready, JoinedGuild joinedGuild, LeftGuild leftGuild)
    {
        _client = client;
        _messageReceived = messageReceived;
        _interactionCreated = interactionCreated;
        _config = config;
        _ready = ready;
        _joinedGuild = joinedGuild;
        _leftGuild = leftGuild;
    }

    public async Task RunAsync()
    {
        var discordToken = _config["tokens:discord"];     // Get the discord token from the config file
        if (string.IsNullOrWhiteSpace(discordToken))
            throw new Exception("Por favor informe o token do bot no arquivo `_config.yml`.");
        
        await _client.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
        // await _client.SetGameAsync("👀");
        await _client.StartAsync();                                // Connect to the websocket
        
        _client.Ready += _ready.HandleEventAsync;
        _client.MessageReceived += _messageReceived.HandleEventAsync;
        _client.InteractionCreated += _interactionCreated.HandleEventAsync;
        _client.JoinedGuild += _joinedGuild.HandleEventAsync;
        _client.LeftGuild += _leftGuild.HandleEventAsync;

        await Task.Delay(-1);
    }
}
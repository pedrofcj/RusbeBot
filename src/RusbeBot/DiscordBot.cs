using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Events.Discord;

namespace RusbeBot;

public class DiscordBot(DiscordSocketClient client, MessageReceived messageReceived, InteractionCreated interactionCreated, IConfigurationRoot config, Ready ready, JoinedGuild joinedGuild, LeftGuild leftGuild)
{
    public async Task RunAsync()
    {
        var discordToken = config["tokens:discord"];     // Get the discord token from the config file
        if (string.IsNullOrWhiteSpace(discordToken))
            throw new Exception("Por favor informe o token do bot no arquivo `_config.yml`.");
        
        await client.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
        // await _client.SetGameAsync("👀");
        await client.StartAsync();                                // Connect to the websocket
        
        client.Ready += ready.HandleEventAsync;
        client.MessageReceived += messageReceived.HandleEventAsync;
        client.InteractionCreated += interactionCreated.HandleEventAsync;
        client.JoinedGuild += joinedGuild.HandleEventAsync;
        client.LeftGuild += leftGuild.HandleEventAsync;

        await Task.Delay(-1);
    }
}
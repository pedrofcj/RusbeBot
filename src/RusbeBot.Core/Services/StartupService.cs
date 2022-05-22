using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace RusbeBot.Core.Services;

public class StartupService
{
    private readonly IServiceProvider _provider;
    public readonly DiscordSocketClient Discord;
    private readonly CommandService _commands;
    private readonly IConfigurationRoot _config;
    private readonly InteractionService _interactionService;

    // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
    public StartupService(
        IServiceProvider provider,
        DiscordSocketClient discord,
        CommandService commands,
        IConfigurationRoot config, 
        InteractionService interactionService)
    {
        _provider = provider;
        _config = config;
        _interactionService = interactionService;
        Discord = discord;
        _commands = commands;
    }

    public async Task StartAsync()
    {
        var discordToken = _config["tokens:discord"];     // Get the discord token from the config file
        if (string.IsNullOrWhiteSpace(discordToken))
            throw new Exception("Por favor informe o token do bot no arquivo `_config.yml`.");

        await Discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
        await Discord.StartAsync();                                // Connect to the websocket

        await _commands.AddModulesAsync(Assembly.GetAssembly(typeof(StartupService)), _provider);     // Load commands and modules into the command service
        await _interactionService.AddModulesAsync(Assembly.GetAssembly(typeof(StartupService)), _provider); // Load commands and modules into the interaction service
    }
}
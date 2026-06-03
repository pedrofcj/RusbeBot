using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Extensions;
using RusbeBot.Data.Interfaces;

namespace RusbeBot.Core.Services;

public class CommandHandler
{
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IConfigurationRoot _config;
    private readonly IServiceProvider _provider;
    private readonly IPrecosService _precosService;
    private readonly InteractionService _interactionService;

    // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
    public CommandHandler(
        DiscordSocketClient discord,
        CommandService commands,
        IConfigurationRoot config,
        IServiceProvider provider,
        IPrecosService precosService, 
        InteractionService interactionService)
    {
        _discord = discord;
        _commands = commands;
        _config = config;
        _provider = provider;
        _precosService = precosService;
        _interactionService = interactionService;

        _discord.MessageReceived += OnMessageReceivedAsync;
        _discord.JoinedGuild += DiscordOnJoinedGuild;
        _discord.LeftGuild += DiscordOnLeftGuild;
        _discord.ButtonExecuted += DiscordOnButtonExecuted;
        _discord.Ready += DiscordOnReady;
    }

    private async Task DiscordOnReady()
    {
        try
        {
            await _interactionService.RegisterCommandsToGuildAsync(553813961363947531);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task DiscordOnButtonExecuted(SocketMessageComponent arg)
    {
        
    }

    private static Task DiscordOnJoinedGuild(SocketGuild arg)
    {
        
        return Task.CompletedTask;
    }

    private static Task DiscordOnLeftGuild(SocketGuild arg)
    {
       
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage s)
    {
        if (s is not SocketUserMessage msg) return;
        if (msg.Author.Id == _discord.CurrentUser.Id) return;     // Ignore self when checking commands

        await msg.CheckPrice(_precosService);

        var context = new SocketCommandContext(_discord, msg);     // Create the command context

        await context.VerificarTaxado();

        var argPos = 0;     // Check if the message has a valid command prefix
        if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
        {
            var result = await _commands.ExecuteAsync(context, argPos, _provider);     // Execute the command

            if (result.IsSuccess)
            {
                var logTags = new List<KeyValuePair<string, string>>
                {
                    new("Tipo", "Comando"),
                    new("Guild Name", context.Guild?.Name ?? string.Empty),
                    new("Guild Id", context.Guild?.Id.ToString() ?? string.Empty),
                    new("Channel Name", context.Channel?.Name ?? string.Empty),
                    new("Channel Id", context.Channel?.Id.ToString() ?? string.Empty),
                    new("User", $"{context.User?.Username}#{context.User?.Discriminator}"),
                    new("User Id", context.User?.Id.ToString() ?? string.Empty),
                };

              
                return;
            }

            //await context.Channel.SendMessageAsync(result.ToString());
        }
    }
}
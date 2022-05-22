using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Events;
using RusbeBot.Core.Extensions;
using RusbeBot.Core.Helpers;
using RusbeBot.Data.Interfaces;
using Sentry;

namespace RusbeBot.Core.Services;

public class CommandHandler
{
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IConfigurationRoot _config;
    private readonly IServiceProvider _provider;
    private readonly IPrecosService _precosService;
    private readonly IMediator _mediator;
    private readonly InteractionService _interactionService;

    // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
    public CommandHandler(
        DiscordSocketClient discord,
        CommandService commands,
        IConfigurationRoot config,
        IServiceProvider provider,
        IPrecosService precosService, 
        IMediator mediator, 
        InteractionService interactionService)
    {
        _discord = discord;
        _commands = commands;
        _config = config;
        _provider = provider;
        _precosService = precosService;
        _mediator = mediator;
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
        switch (arg.Data.CustomId)
        {
            case { } s when s.StartsWith("getActors"):
                await _mediator.Publish(new GetActorsEvent(arg));
                break;
        }
    }

    private static Task DiscordOnJoinedGuild(SocketGuild arg)
    {
        SentrySdk.CaptureMessage($"Entrou no server {arg.Name} ({arg.Id})", scope =>
        {
            scope.SetTags(new List<KeyValuePair<string, string>>
            {
                new("Tipo", "Entrou")
            });
        });
        return Task.CompletedTask;
    }

    private static Task DiscordOnLeftGuild(SocketGuild arg)
    {
        SentrySdk.CaptureMessage($"Saiu no server {arg.Name} ({arg.Id})", scope =>
        {
            scope.SetTags(new List<KeyValuePair<string, string>>
            {
                new("Tipo", "Saiu")
            });
        });
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

                SentrySdk.CaptureMessage($"Comando executado: {msg.Content} {Environment.NewLine}" +
                                         $"Mensagem: '{msg.Content}'",
                    scope =>
                    {
                        scope.SetTags(logTags);
                    });
                return;
            }

            //await context.Channel.SendMessageAsync(result.ToString());
            SentryHelper.Log($"Erro ao tentar executar o comando da mensagem '{msg.Content}'", context, SentryLevel.Error);
        }
    }
}
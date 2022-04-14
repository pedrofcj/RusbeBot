using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Extensions;
using RusbeBot.Helpers;
using Sentry;

namespace RusbeBot.Services;

public class CommandHandler
{
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IConfigurationRoot _config;
    private readonly IServiceProvider _provider;
    private readonly IPrecosService _precosService;

    // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
    public CommandHandler(
        DiscordSocketClient discord,
        CommandService commands,
        IConfigurationRoot config,
        IServiceProvider provider,
        IPrecosService precosService)
    {
        _discord = discord;
        _commands = commands;
        _config = config;
        _provider = provider;
        _precosService = precosService;

        _discord.MessageReceived += OnMessageReceivedAsync;
        _discord.JoinedGuild += DiscordOnJoinedGuild;
        _discord.LeftGuild += DiscordOnLeftGuild;
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
                    new("Guild Name", context.Guild?.Name),
                    new("Guild Id", context.Guild?.Id.ToString()),
                    new("Channel Name", context.Channel?.Name),
                    new("Channel Id", context.Channel?.Id.ToString()),
                    new("User", $"{context.User?.Username}#{context.User?.Discriminator}"),
                    new("User Id", context.User?.Id.ToString()),
                };

                SentrySdk.CaptureMessage($"Comando executado: {msg.Content} {Environment.NewLine}" +
                                         $"Mensagem: '{msg.Content}'",
                    scope =>
                    {
                        scope.SetTags(logTags);
                    });
                return;
            }

            await context.Channel.SendMessageAsync(result.ToString());
            SentryHelper.Log($"Erro ao tentar executar o comando da mensagem '{msg.Content}'", context, SentryLevel.Error);
        }
    }
}
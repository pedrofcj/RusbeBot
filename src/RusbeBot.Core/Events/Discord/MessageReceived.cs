using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RusbeBot.Core.Extensions;
using RusbeBot.Core.Helpers;
using RusbeBot.Data.Interfaces;

namespace RusbeBot.Core.Events.Discord;

public class MessageReceived
{
    private readonly DiscordSocketClient _discord;
    private readonly CommandService _commands;
    private readonly IConfigurationRoot _config;
    private readonly IPrecosService _precosService;
    private readonly IServiceProvider _provider;

    public MessageReceived(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IPrecosService precosService, IServiceProvider provider)
    {
        _discord = discord;
        _commands = commands;
        _config = config;
        _precosService = precosService;
        _provider = provider;
    }

    public async Task HandleEventAsync(SocketMessage socketMessage)
    {
        if (socketMessage.Author.IsBot) return;
        if (socketMessage is not SocketUserMessage msg) return;
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

        }
    }
}
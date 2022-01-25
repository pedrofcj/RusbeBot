using System;
using System.Threading.Tasks;
using Data.Interfaces;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using TheLostBot.Extensions;


namespace TheLostBot.Services
{
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
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return;     // Ignore self when checking commands

            await msg.VerificarTaxado();
            await msg.CheckPrice(_precosService);

            var context = new SocketCommandContext(_discord, msg);     // Create the command context

            var argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);     // Execute the command

                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}
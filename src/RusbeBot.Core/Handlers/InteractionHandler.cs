using System.Reflection;
using Discord.Commands;
using Discord.Interactions;
using RusbeBot.Core.Services;

namespace RusbeBot.Core.Handlers;

public class InteractionHandler
{
    private readonly CommandService _commandService;
    private readonly IServiceProvider _serviceProvider;
    private readonly InteractionService _interactionService;

    public InteractionHandler(CommandService commandService, IServiceProvider serviceProvider, InteractionService interactionService)
    {
        _commandService = commandService;
        _serviceProvider = serviceProvider;
        _interactionService = interactionService;
    }

    public async Task InitializeAsync()
    {
        await _commandService.AddModulesAsync(Assembly.GetAssembly(typeof(StartupService)), _serviceProvider);     // Load commands and modules into the command service
        await _interactionService.AddModulesAsync(Assembly.GetAssembly(typeof(StartupService)), _serviceProvider); // Load commands and modules into the interaction service
    }
}
using System;
using Discord.Commands;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using RusbeBot.Core.Events;
using RusbeBot.Core.Handlers;
using RusbeBot.Core.Services;
using RusbeBot.Data.Implementation.SQlite;
using RusbeBot.Data.Interfaces;

namespace RusbeBot.ServiceCollection;

public static class ServiceCollectionExtension
{
    public static void AddBasicServices(this IServiceCollection services)
    {
        services.AddSingleton<DiscordBot>();
        services.AddSingleton<CommandService>();
        services.AddSingleton<CommandHandler>();
        services.AddSingleton<StartupService>();
        services.AddSingleton<LoggingService>();
        services.AddSingleton<Random>();
        services.AddSingleton<InteractionService>();
        services.AddSingleton<InteractionHandler>();
        services.AddSingleton<InteractionCreated>(); 
        services.AddSingleton<MessageReceived>();
        services.AddSingleton<Ready>();
    }
    
    public static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<IPicsService, SqlitePicsService>();
        services.AddSingleton<IPrecosService, SqlitePrecosService>();
        services.AddSingleton<IAllowedRolesConfigService, SqliteAllowedRolesConfigService>();
        services.AddSingleton<IAllowedChannelsConfigService, SqliteAllowedChannelsConfigService>();
        services.AddSingleton<IModeradorService, SqliteModeradorService>();
    }
}
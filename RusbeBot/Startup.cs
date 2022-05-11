using Data.Implementation.SQlite;
using Data.Interfaces;
using Data.Values;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RusbeBot.Services;
using Sentry;
using System;
using System.Threading.Tasks;

namespace RusbeBot;

public class Startup
{
    private IConfigurationRoot Configuration { get; }

    public Startup(string[] args)
    {
        var builder = new ConfigurationBuilder()        // Create a new instance of the config builder
            .SetBasePath(AppContext.BaseDirectory)      // Specify the default location for the config file
            .AddYamlFile("_config.yml");                // Add this (yaml encoded) file to the configuration
        try
        {
            Configuration = builder.Build();                // Build the configuration
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public static async Task RunAsync(string[] args)
    {
        var startup = new Startup(args);
        await startup.RunAsync();
    }

    private async Task RunAsync()
    {
        AwsCredentials.SetCredentials(Configuration["aws:accessKeyId"], Configuration["aws:secretAccessKey"]);

        var sentryToken = Configuration["tokens:sentry"];

        if (!string.IsNullOrWhiteSpace(sentryToken))
        {
            SentrySdk.Init(options =>
            {
                options.Dsn = sentryToken;
                options.Debug = true;
                options.TracesSampleRate = 1.0;
                options.AttachStacktrace = true;
            });
        }

        var services = new ServiceCollection();             // Create a new instance of a service collection
        ConfigureServices(services);

        var provider = services.BuildServiceProvider();     // Build the service provider
        provider.GetRequiredService<LoggingService>();      // Start the logging service
        provider.GetRequiredService<CommandHandler>(); 		// Start the command handler service

        await provider.GetRequiredService<StartupService>().StartAsync();       // Start the startup service
        await Task.Delay(-1);                               // Keep the program alive
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
        {                                           // Add discord to the collection
            LogLevel = LogSeverity.Verbose,         // Tell the logger to give Verbose amount of info
            MessageCacheSize = 1000,                // Cache 1,000 messages per channel
            AlwaysDownloadUsers = true,             // Download users to cache
            GatewayIntents = GatewayIntents.All,    // Set all the intents available (a bit overkill, but the bot is growing fast)

        }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {                                       // Add the command service to the collection
                LogLevel = LogSeverity.Verbose,     // Tell the logger to give Verbose amount of info
                DefaultRunMode = RunMode.Async,     // Force all commands to run async by default
            }))
            .AddSingleton<CommandHandler>()         // Add the command handler to the collection
            .AddSingleton<StartupService>()         // Add StartupService to the collection
            .AddSingleton<LoggingService>()         // Add LoggingService to the collection
            .AddSingleton<IPicsService, SqlitePicsService>() // Add Picture service to collection
            .AddSingleton<IPrecosService, SqlitePrecosService>() // Add Picture service to collection
            .AddSingleton<IAllowedRolesConfigService, SqliteAllowedRolesConfigService>() // Add roles config service to collection
            .AddSingleton<IAllowedChannelsConfigService, SqliteAllowedChannelsConfigService>() // Add channels config service to collection
            .AddSingleton<IModeradorService, SqliteModeradorService>() // Add Moderador config service to collection
            .AddSingleton<Random>()                 // Add random to the collection
            .AddSingleton(Configuration);           // Add the configuration to the collection

    }
}
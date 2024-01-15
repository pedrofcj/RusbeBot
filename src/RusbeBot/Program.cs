using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using IMDbApiLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RusbeBot.Core.Handlers;
using RusbeBot.Core.Helpers;
using RusbeBot.Core.Services;
using RusbeBot.ServiceCollection;
using Sentry;
using Serilog;

namespace RusbeBot;

internal class Program
{
    private static readonly IConfigurationRoot Configuration = BuildConfiguration();

    public static async Task Main(string[] args)
    {
        var serviceDescriptors = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        ConfigureServices(serviceDescriptors);

        var serviceProvider = serviceDescriptors.BuildServiceProvider();

        ConfigureRequiredServices(serviceProvider);

        SentrySdk.Init(o =>
        {
            o.Dsn = Configuration["tokens:sentry"];
            o.Debug = true;
            o.Environment = Configuration["environment"];
            o.AutoSessionTracking = true;
            o.IsGlobalModeEnabled = true;
            o.EnableTracing = true;
            o.StackTraceMode = StackTraceMode.Enhanced;
        });

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        await serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
        serviceProvider.GetRequiredService<DiscordBot>().RunAsync().GetAwaiter().GetResult();
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder() // Create a new instance of the config builder
            .SetBasePath(AppContext.BaseDirectory) // Specify the default location for the config file
            .AddYamlFile("_config.yml"); // Add this (yaml encoded) file to the configuration
        try
        {
            var configuration = builder.Build(); // Build the configuration
            return configuration;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static DiscordSocketClient CreateDiscordSocketClient()
    {
        var client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose, // Tell the logger to give Verbose amount of info
            MessageCacheSize = 1000, // Cache 1,000 messages per channel
            AlwaysDownloadUsers = true, // Download users to cache
            GatewayIntents = GatewayIntents.All, // Set all the intents available (a bit overkill, but you can change it later)
        });

        return client;
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        var imdbApiKey = Configuration["tokens:imdbApiKey"];

        AwsHelper.Start(Configuration);
        
        serviceCollection.AddSingleton(CreateDiscordSocketClient());
        serviceCollection.AddBasicServices();
        serviceCollection.AddDatabaseServices();
        serviceCollection.AddSingleton(Configuration);
        serviceCollection.AddSingleton(new ApiLib(imdbApiKey));
        serviceCollection.AddLogging(configure => configure.AddSerilog());

    }

    private static void ConfigureRequiredServices(IServiceProvider serviceProvider)
    {
        serviceProvider.GetRequiredService<DiscordSocketClient>();
        serviceProvider.GetRequiredService<InteractionService>();
        serviceProvider.GetRequiredService<IServiceProvider>();
        serviceProvider.GetRequiredService<LoggingService>();
    }
    
}

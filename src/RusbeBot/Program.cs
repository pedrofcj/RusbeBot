using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using IMDbApiLib;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RusbeBot.Core.Handlers;
using RusbeBot.Core.Helpers;
using RusbeBot.Core.Services;
using RusbeBot.ServiceCollection;

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

        SentryHelper.Start(Configuration);
        AwsHelper.Start(Configuration);
        
        serviceCollection.AddSingleton(CreateDiscordSocketClient());
        serviceCollection.AddBasicServices();
        serviceCollection.AddDatabaseServices();
        serviceCollection.AddSingleton(Configuration);
        serviceCollection.AddSingleton(new ApiLib(imdbApiKey));
        serviceCollection.AddMediatR(typeof(Program));
    }

    private static void ConfigureRequiredServices(IServiceProvider serviceProvider)
    {
        serviceProvider.GetRequiredService<DiscordSocketClient>();
        serviceProvider.GetRequiredService<InteractionService>();
        serviceProvider.GetRequiredService<IServiceProvider>();
        serviceProvider.GetRequiredService<LoggingService>();
    }
    
}

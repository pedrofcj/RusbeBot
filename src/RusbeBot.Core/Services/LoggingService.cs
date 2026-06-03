using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using RusbeBot.Core.Extensions;

namespace RusbeBot.Core.Services;

public class LoggingService
{
    private string LogDirectory { get; }
    private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");
    private readonly ILogger<LoggingService> _logger;

    // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
    public LoggingService(DiscordSocketClient discord, CommandService commands, ILogger<LoggingService> logger)
    {
        _logger = logger;
        LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

        discord.Log += OnLogAsync;
        commands.Log += OnLogAsync;

        discord.Log += SerilogDiscordLogAsync;
        commands.Log += SerilogDiscordLogAsync;
    }

    private Task OnLogAsync(LogMessage msg)
    {
        if (!Directory.Exists(LogDirectory))     // Create the log directory if it doesn't exist
            Directory.CreateDirectory(LogDirectory);
        if (!File.Exists(LogFile))               // Create today's log file if it doesn't exist
            File.Create(LogFile).Dispose();

        var logText = FormatMessage(msg);

        try
        {
            File.AppendAllText(LogFile, logText + "\n");     // Write the log text to a file
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Console.Out.WriteLineAsync(logText);       // Write the log text to the console
    }

    private static string FormatMessage(LogMessage msg) => $"{DateTime.UtcNow:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";

    #region Sentry

    private static bool ShouldLog(string logText)
    {
        if (logText.ToLowerInvariant().Contains("consider removing the intent from your config".ToLowerInvariant()))
            return false;

        if (logText.ToLowerInvariant().Contains("WebSocket connection was closed".ToLowerInvariant()))
            return false;

        if (logText.ToLowerInvariant().Contains("Server requested a reconnect".ToLowerInvariant()))
            return false;


        return true;
    }

    private Task SerilogDiscordLogAsync(LogMessage msg)
    {
        _logger.Log(msg.Severity.ToSerilogLevel(), FormatMessage(msg));
        return Task.CompletedTask;
    }

    #endregion

}
using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Sentry;
using TheLostBot.Extensions;

namespace TheLostBot.Services;

public class LoggingService
{
    private string LogDirectory { get; }
    private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");

    // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
    public LoggingService(DiscordSocketClient discord, CommandService commands)
    {
        LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

        discord.Log += OnLogAsync;
        commands.Log += OnLogAsync;

        discord.Log += SentryDiscordLogAsync;
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
 
    private static Task SentryDiscordLogAsync(LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Critical:
            case LogSeverity.Error:
            case LogSeverity.Warning:
                var logText = FormatMessage(msg);
                if (logText.ToLowerInvariant().Contains("consider removing the intent from your config".ToLowerInvariant()))
                    break;

                SentrySdk.CaptureMessage($"{FormatMessage(msg)} {Environment.NewLine}Raw: {JsonConvert.SerializeObject(msg)}", msg.Severity.ToSentryLevel());
                break;
        }

        return Task.CompletedTask;
    }

    #endregion

}
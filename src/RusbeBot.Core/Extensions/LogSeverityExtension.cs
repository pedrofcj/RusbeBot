using Discord;
using Microsoft.Extensions.Logging;

namespace RusbeBot.Core.Extensions;

public static class LogSeverityExtension
{
    public static LogLevel ToSerilogLevel(this LogSeverity logSeverity)
    {
        switch (logSeverity)
        {
            case LogSeverity.Critical:
                return LogLevel.Critical;

            case LogSeverity.Error:
                return LogLevel.Error;

            case LogSeverity.Warning:
                return LogLevel.Warning;

            case LogSeverity.Info:
                return LogLevel.Information;

            case LogSeverity.Verbose:
            case LogSeverity.Debug:
                return LogLevel.Debug;

            default:
                throw new ArgumentOutOfRangeException(nameof(logSeverity), logSeverity, null);
        }
    }
}
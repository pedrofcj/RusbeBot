using Discord;
using Sentry;
using System;

namespace RusbeBot.Extensions;

public static class LogSeverityExtension
{
    public static SentryLevel ToSentryLevel(this LogSeverity logSeverity)
    {
        switch (logSeverity)
        {
            case LogSeverity.Critical:
                return SentryLevel.Fatal;

            case LogSeverity.Error:
                return SentryLevel.Error;

            case LogSeverity.Warning:
                return SentryLevel.Warning;

            case LogSeverity.Info:
                return SentryLevel.Info;

            case LogSeverity.Verbose:
            case LogSeverity.Debug:
                return SentryLevel.Debug;

            default:
                throw new ArgumentOutOfRangeException(nameof(logSeverity), logSeverity, null);
        }
    }
}
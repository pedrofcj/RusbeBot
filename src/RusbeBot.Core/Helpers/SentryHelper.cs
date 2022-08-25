using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Sentry;

namespace RusbeBot.Core.Helpers;

public static class SentryHelper
{
    public static void Start(IConfigurationRoot configuration)
    {
        var sentryToken = configuration["tokens:sentry"];

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
    }
    
    public static void Log(string msg, ICommandContext? context, SentryLevel level = SentryLevel.Info, List<KeyValuePair<string, string>>? extraScope = null)
    {
        SentrySdk.CaptureMessage(msg, scope => scope.ConfigureScope(context, extraScope), level);
    }

    private static void ConfigureScope(this Scope scope, ICommandContext? context, List<KeyValuePair<string, string>>? extraScope = null)
    {
        extraScope ??= new List<KeyValuePair<string, string>>();
        extraScope.AddRange(new List<KeyValuePair<string, string>>
        {
            new("Guild Name", context?.Guild?.Name ?? string.Empty),
            new("Guild Id", context?.Guild?.Id.ToString() ?? string.Empty),
            new("Channel Name", context?.Channel?.Name ?? string.Empty),
            new("Channel Id", context?.Channel?.Id.ToString() ?? string.Empty),
            new("User", $"{context?.User?.Username}#{context?.User?.Discriminator}"),
            new("User Id", context?.User?.Id.ToString() ?? string.Empty),
        });

        if (context?.User is IGuildUser user)
        {
            var roleNames = (from roleId in user.RoleIds select context.Guild?.GetRole(roleId) into role where role != null select role.Name).ToList();

            extraScope.AddRange(new List<KeyValuePair<string, string>>
            {
                new("Role Ids", string.Join(',', user.RoleIds)),
                new("Role Names", string.Join(',', roleNames))
            });
        }

        scope.SetTags(extraScope);
    }
}
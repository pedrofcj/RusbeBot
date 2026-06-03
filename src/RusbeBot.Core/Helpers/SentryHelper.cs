using Discord.Commands;
using Discord;
using Sentry;

namespace RusbeBot.Core.Helpers;

public static class SentryHelper
{
    public static void Log(string msg, ICommandContext? context, SentryLevel level = SentryLevel.Info, List<KeyValuePair<string, string>>? extraScope = null)
    {
        SentrySdk.CaptureMessage(msg, scope => scope.ConfigureScope(context, extraScope), level);
    }

    private static void ConfigureScope(this Scope scope, ICommandContext? context, List<KeyValuePair<string, string>>? extraScope = null)
    {
        if (context == null) return;

        extraScope ??= new List<KeyValuePair<string, string>>();
        extraScope.AddRange(new List<KeyValuePair<string, string>>
        {
            new("Guild Name", context.Guild?.Name ?? "Unable to log the Guild Name"),
            new("Guild Id", context.Guild?.Id.ToString() ?? "Unable to log the Guild Id"),
            new("Channel Name", context.Channel ?.Name ?? "Unable to log the Channel Name"),
            new("Channel Id", context.Channel ?.Id.ToString() ?? "Unable to log the Channel Id"),
            new("User", $"{context.User?.Username}#{context.User?.Discriminator}"),
            new("User Id", context.User ?.Id.ToString() ?? "Unable to log the User Id"),
        });

        if (context.User is IGuildUser user)
        {
            var roleNames = (from roleId in user.RoleIds select context.Guild.GetRole(roleId) into role where role != null select role.Name).ToList();
            extraScope.AddRange(new List<KeyValuePair<string, string>>
            {
                new("Role Ids", string.Join(',', user.RoleIds)),
                new("Role Names", string.Join(',', roleNames))
            });
        }

        scope.SetTags(extraScope);
    }
}
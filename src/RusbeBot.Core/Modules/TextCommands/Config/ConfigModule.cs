using Discord.Commands;
using RusbeBot.Data.Interfaces;

namespace RusbeBot.Core.Modules.TextCommands.Config;

[Name("Config")]
[Group("config")]
public class ConfigModule : ModuleBase<SocketCommandContext>
{

    protected readonly IAllowedChannelsConfigService AllowedChannelsConfigService;
    protected readonly IAllowedRolesConfigService AllowedRolesConfigService;
    public ConfigModule(IAllowedChannelsConfigService allowedChannelsConfigService, IAllowedRolesConfigService allowedRolesConfigService)
    {
        AllowedChannelsConfigService = allowedChannelsConfigService;
        AllowedRolesConfigService = allowedRolesConfigService;
    }
}
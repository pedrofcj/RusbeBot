using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Implementation.SQlite;

public class SqliteAllowedChannelsConfigService : SqliteBaseService<AllowedChannelsConfigModel>, IAllowedChannelsConfigService
{
    public SqliteAllowedChannelsConfigService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }

    public async Task<List<AllowedChannelsConfigModel>> GetAllowedChannelsByCommandAndGuild(string command, string guildId)
    {
        var response = await Db.Table<AllowedChannelsConfigModel>().Where(model => model.CommandName == command && model.GuildId == guildId).ToListAsync();
        return response;
    }
}
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Implementation.SQlite;

public class SqliteAllowedRolesConfigService : SqliteBaseService<AllowedRolesConfigModel>, IAllowedRolesConfigService
{
    public SqliteAllowedRolesConfigService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }

    public async Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId)
    {
        var response = await Db.Table<AllowedRolesConfigModel>().Where(model => model.CommandName == command && model.GuildId == guildId).ToListAsync();
        return response;
    }

}
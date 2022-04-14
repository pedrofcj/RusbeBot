using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;

namespace Data.Implementation.DynamoDb;

public class DynamoAllowedConfigService : DynamoBaseService<AllowedRolesConfigModel>, IAllowedRolesConfigService
{
    public async Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId)
    {
        var record = await GetAllAsync();
        return record.Where(model => model.CommandName == command && model.GuildId == guildId).ToList();
    }
}
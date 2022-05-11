using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implementation.DynamoDb;

public class DynamoAllowedRolesConfigService : DynamoBaseService<AllowedRolesConfigModel>, IAllowedRolesConfigService
{
    public async Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId)
    {
        var filters = new List<ScanCondition>
        {
            new(nameof(AllowedRolesConfigModel.CommandName), ScanOperator.Equal, command),
            new(nameof(AllowedRolesConfigModel.GuildId), ScanOperator.Equal, guildId)
        };

        var result = await GetByFilters(filters);
        return result.ToList();
    }
}
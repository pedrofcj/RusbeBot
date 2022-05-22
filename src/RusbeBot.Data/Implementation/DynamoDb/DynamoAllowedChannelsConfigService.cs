using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Implementation.DynamoDb;

public class DynamoAllowedChannelsConfigService : DynamoBaseService<AllowedChannelsConfigModel>, IAllowedChannelsConfigService
{
    public async Task<List<AllowedChannelsConfigModel>> GetAllowedChannelsByCommandAndGuild(string command, string guildId)
    {
        var filters = new List<ScanCondition>
        {
            new(nameof(AllowedChannelsConfigModel.CommandName), ScanOperator.Equal, command),
            new(nameof(AllowedChannelsConfigModel.GuildId), ScanOperator.Equal, guildId)
        };

        var result = await GetByFilters(filters);
        return result.ToList();

        //var record = await GetAllAsync();
        //return record.Where(model => model.CommandName == command && model.GuildId == guildId).ToList();
    }
}
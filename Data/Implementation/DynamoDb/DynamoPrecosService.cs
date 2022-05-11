using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implementation.DynamoDb;

public class DynamoPrecosService : DynamoBaseService<PrecosModel>, IPrecosService
{
    public async Task<PrecosModel> GetByDate(DateTime date, string guildId)
    {

        var filters = new List<ScanCondition>
        {
            new(nameof(PrecosModel.Data), ScanOperator.Equal, date),
            new(nameof(PrecosModel.GuildId), ScanOperator.Equal, guildId)

        };

        var result = await GetByFilters(filters);
        return result.FirstOrDefault();

        //var records = await GetAllAsync();
        //return records.FirstOrDefault(d => d.Data == date && d.GuildId == guildId);

    }
}
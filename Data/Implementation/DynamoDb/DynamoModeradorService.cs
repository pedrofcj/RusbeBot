using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Data.Interfaces;
using Data.Models;

namespace Data.Implementation.DynamoDb;

public class DynamoModeradorService : DynamoBaseService<ModeradorModel>, IModeradorService
{
    public async Task<ModeradorModel> GetModeradorByUserIdAsync(string userId)
    {
        var filters = new List<ScanCondition>
        {
            new(nameof(ModeradorModel.UserId), ScanOperator.Equal, userId)
        };

        var result = await GetByFilters(filters);
        return result.FirstOrDefault();
    }
}
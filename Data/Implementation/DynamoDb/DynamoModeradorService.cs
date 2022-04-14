using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;

namespace Data.Implementation.DynamoDb;

public class DynamoModeradorService : DynamoBaseService<ModeradorModel>, IModeradorService
{
    public async Task<ModeradorModel> GetModeradorByUserIdAsync(string userId)
    {
        var records = await GetAllAsync();
        return records.FirstOrDefault(model => model.UserId == userId);
    }
}
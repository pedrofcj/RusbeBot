using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Data.Implementation.SQlite;

public class SqliteModeradorService : SqliteBaseService<ModeradorModel>, IModeradorService
{
    public SqliteModeradorService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }

    public async Task<ModeradorModel> GetModeradorByUserIdAsync(string userId)
    {
        var response = await Db.Table<ModeradorModel>().FirstOrDefaultAsync(model => model.UserId == userId);
        return response;
    }

}
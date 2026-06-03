using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Implementation.SQlite;

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
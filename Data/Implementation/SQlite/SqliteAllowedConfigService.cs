using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Implementation.SQlite
{
    public class SqliteAllowedConfigService : SqliteBaseService<AllowedRolesConfigModel>, IAllowedRolesConfigService
    {
        public SqliteAllowedConfigService(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
        }

        public async Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId)
        {
            var response = await Db.Table<AllowedRolesConfigModel>().Where(model => model.CommandName == command && model.GuildId == guildId).ToListAsync();
            return response;
        }
     
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces;

public interface IAllowedRolesConfigService : IDbService<AllowedRolesConfigModel>
{
    Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId);
}
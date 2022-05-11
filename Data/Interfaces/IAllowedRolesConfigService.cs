using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IAllowedRolesConfigService : IDbService<AllowedRolesConfigModel>
{
    Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId);
}
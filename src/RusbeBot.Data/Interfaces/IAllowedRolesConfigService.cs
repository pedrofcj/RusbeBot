using System.Collections.Generic;
using System.Threading.Tasks;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Interfaces;

public interface IAllowedRolesConfigService : IDbService<AllowedRolesConfigModel>
{
    Task<List<AllowedRolesConfigModel>> GetAllowedRolesByCommandAndGuild(string command, string guildId);
}
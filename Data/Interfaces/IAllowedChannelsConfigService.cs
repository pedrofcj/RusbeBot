using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IAllowedChannelsConfigService : IDbService<AllowedChannelsConfigModel>
{
    Task<List<AllowedChannelsConfigModel>> GetAllowedChannelsByCommandAndGuild(string command, string guildId);
}
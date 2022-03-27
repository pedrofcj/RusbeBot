using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface IAllowedChannelsConfigService : IDbService<AllowedChannelsConfigModel>
    {
        Task<List<AllowedChannelsConfigModel>> GetAllowedChannelsByCommandAndGuild(string command, string guildId);
    }
}
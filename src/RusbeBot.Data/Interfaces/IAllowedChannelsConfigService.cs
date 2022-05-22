using System.Collections.Generic;
using System.Threading.Tasks;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Interfaces;

public interface IAllowedChannelsConfigService : IDbService<AllowedChannelsConfigModel>
{
    Task<List<AllowedChannelsConfigModel>> GetAllowedChannelsByCommandAndGuild(string command, string guildId);
}
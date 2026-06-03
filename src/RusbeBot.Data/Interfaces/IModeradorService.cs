using System.Threading.Tasks;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Interfaces;

public interface IModeradorService : IDbService<ModeradorModel>
{
    Task<ModeradorModel> GetModeradorByUserIdAsync(string userId);


}
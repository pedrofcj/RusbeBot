using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces;

public interface IModeradorService : IDbService<ModeradorModel>
{
    Task<ModeradorModel> GetModeradorByUserIdAsync(string userId);

    
}
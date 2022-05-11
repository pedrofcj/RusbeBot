using Data.Models;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IModeradorService : IDbService<ModeradorModel>
{
    Task<ModeradorModel> GetModeradorByUserIdAsync(string userId);


}
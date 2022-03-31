using System;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface IPrecosService : IDbService<PrecosModel>
    {
        Task<PrecosModel> GetByDate(DateTime date, string guildId);
    }
}
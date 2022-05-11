using Data.Models;
using System;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IPrecosService : IDbService<PrecosModel>
{
    Task<PrecosModel> GetByDate(DateTime date, string guildId);
}
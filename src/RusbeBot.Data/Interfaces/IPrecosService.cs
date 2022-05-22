using System;
using System.Threading.Tasks;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Interfaces;

public interface IPrecosService : IDbService<PrecosModel>
{
    Task<PrecosModel> GetByDate(DateTime date, string guildId);
}
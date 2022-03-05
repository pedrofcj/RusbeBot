using System;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Implementation.SQlite
{
    public class SqlitePrecosService : SqliteBaseService<PrecosModel>, IPrecosService
    {
        public SqlitePrecosService(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
        }

        public async Task<PrecosModel> GetByDate(DateTime date)
        {
            var result = await Db.FindAsync<PrecosModel>((model => model.Data == date));
            return result;
        }
    }
}
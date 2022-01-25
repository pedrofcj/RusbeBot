using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;

namespace Data.Implementation.DynamoDb
{
    public class DynamoPrecosService : DynamoBaseService<PrecosModel>, IPrecosService
    {
        public async Task<PrecosModel> GetByDate(DateTime date)
        {
            var records = await GetAllAsync();
            
            return records.FirstOrDefault(d=>d.Data == date);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IDbService<T>
{
    Task<string> InsertAsync(T model);
    Task<string> UpdateAsync(T model);
    Task<T> GetByIdAsync(string key);
    Task DeleteAsync(string key);
    Task<List<T>> GetAllAsync();
}
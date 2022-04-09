using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;
using SQLite;

namespace Data.Implementation.SQlite;

public class SqliteBaseService<T>: IDbService<T> where T : BaseModel, new()
{
    protected readonly SQLiteAsyncConnection Db;

    public SqliteBaseService(IConfigurationRoot configurationRoot)
    {
        var databasePath = configurationRoot["sqlite-file"];
        Db = new SQLiteAsyncConnection(databasePath);
        Db.CreateTableAsync(typeof(T)).Wait();
    }

    public async Task<string> InsertAsync(T model)
    {
        await Db.InsertAsync(model);
        return model.Id;
    }

    public async Task<string> UpdateAsync(T model)
    {
        await Db.UpdateAsync(model);
        return model.Id;
    }

    public async Task<T> GetByIdAsync(string key)
    {
        var record = await Db.FindAsync<T>(key);
        if (record == null)
            throw new KeyNotFoundException($"A record with key {key} was not found");

        return record;
    }

    public async Task DeleteAsync(string key)
    {
        await Db.DeleteAsync<T>(key);
    }

    public async Task<List<T>> GetAllAsync()
    {
        var scanResult = await Db.Table<T>().ToListAsync();
        return scanResult;
    }

}
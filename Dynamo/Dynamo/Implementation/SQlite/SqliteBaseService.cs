using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Data.Models;
using SQLite;

namespace Data.Implementation.SQlite
{
    public class SqliteBaseService<T> where T : BaseModel, new()
    {
        private static readonly string DatabasePath = Path.Combine(Environment.CurrentDirectory, "TheLostBot.db");
        protected SQLiteAsyncConnection Db = new SQLiteAsyncConnection(DatabasePath);

        public SqliteBaseService()
        {
            Db.CreateTableAsync(typeof(T)).GetAwaiter().GetResult();
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
}
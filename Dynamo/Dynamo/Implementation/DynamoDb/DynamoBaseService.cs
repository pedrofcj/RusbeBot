using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Data.Models;

namespace Data.Implementation.DynamoDb
{
    public class DynamoBaseService<T> where T : BaseModel
    {
        protected readonly AmazonDynamoDBClient Client;

        public DynamoBaseService(string regionEndpoint = "eu-west-1")
        {
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(regionEndpoint),
            };

            Client = new AmazonDynamoDBClient(Values.AwsCredentials.BasicAwsCredentials, config);

        }

        public async Task<string> InsertAsync(T model)
        {
            using (var context = new DynamoDBContext(Client))
            {
                await context.SaveAsync(model);
            }

            return model.Id;
        }

        public async Task<string> UpdateAsync(T model)
        {
            using (var context = new DynamoDBContext(Client))
            {
                var record = await context.LoadAsync<T>(model.Id);
                if (record == null)
                    throw new KeyNotFoundException($"A record with key {model.Id} was not found");

                await context.SaveAsync(model);
            }

            return model.Id;
        }

        public async Task<T> GetByIdAsync(string key)
        {
            using var context = new DynamoDBContext(Client);

            var record = await context.LoadAsync<T>(key);
            if (record == null)
                throw new KeyNotFoundException($"A record with key {key} was not found");

            return record;
        }

        public async Task DeleteAsync(string key)
        {
            using var context = new DynamoDBContext(Client);

            var record = await context.LoadAsync<T>(key);
            if (record == null)
                throw new KeyNotFoundException($"A record with key {key} was not found");

            await context.DeleteAsync(record);
        }

        public async Task<List<T>> GetAllAsync()
        {
            using var context = new DynamoDBContext(Client);

            var scanResult = await context.ScanAsync<T>(new List<ScanCondition>()).GetNextSetAsync();
            return scanResult;
        }


    }
}
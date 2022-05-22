using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Implementation.DynamoDb;

public class DynamoBaseService<T> : IDbService<T> where T : BaseModel
{
    protected readonly AmazonDynamoDBClient Client;
    protected readonly DynamoDBContext Context;

    public DynamoBaseService(string regionEndpoint = "us-east-1")
    {
        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(regionEndpoint),
        };

        Client = new AmazonDynamoDBClient(Values.AwsCredentials.BasicAwsCredentials, config);
        Context = new DynamoDBContext(Client);
    }

    public async Task<string> InsertAsync(T model)
    {
        await Context.SaveAsync(model);
        return model.Id;
    }

    public async Task<string> UpdateAsync(T model)
    {
        var record = await Context.LoadAsync<T>(model.Id);
        if (record == null)
            throw new KeyNotFoundException($"A record with key {model.Id} was not found");

        await Context.SaveAsync(model);

        return model.Id;
    }

    public async Task<T> GetByIdAsync(string key)
    {
        var record = await Context.LoadAsync<T>(key);
        if (record == null)
            throw new KeyNotFoundException($"A record with key {key} was not found");

        return record;
    }

    public async Task DeleteAsync(string key)
    {
        var record = await Context.LoadAsync<T>(key);
        if (record == null)
            throw new KeyNotFoundException($"A record with key {key} was not found");

        await Context.DeleteAsync(record);
    }

    public async Task<List<T>> GetAllAsync()
    {
        var scanResult = await Context.ScanAsync<T>(new List<ScanCondition>()).GetNextSetAsync();
        return scanResult;
    }

    protected async Task<List<T>> GetByFilters(List<ScanCondition> conditions)
    {
        var scanResult = await Context.ScanAsync<T>(conditions).GetNextSetAsync();
        return scanResult;
    }

}

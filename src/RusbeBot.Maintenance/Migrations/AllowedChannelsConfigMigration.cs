using RusbeBot.Data.Implementation.DynamoDb;
using RusbeBot.Data.Implementation.SQlite;

namespace RusbeBot.Maintenance.Migrations;

public static class AllowedChannelsConfigMigration
{
    public static async Task SqliteToDynamo(DynamoAllowedChannelsConfigService dynamoService, SqliteAllowedChannelsConfigService sqliteService)
    {
        await ClearDynamo(dynamoService);

        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.InsertAsync(config);
        }
    }

    public static async Task DynamoToSqlite(DynamoAllowedChannelsConfigService dynamoService, SqliteAllowedChannelsConfigService sqliteService)
    {
        await ClearSqlite(sqliteService);
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.InsertAsync(config);
        }
    }

    private static async Task ClearDynamo(DynamoAllowedChannelsConfigService dynamoService)
    {
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.DeleteAsync(config.Id);
        }
    }

    private static async Task ClearSqlite(SqliteAllowedChannelsConfigService sqliteService)
    {
        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.DeleteAsync(config.Id);
        }
    }
}
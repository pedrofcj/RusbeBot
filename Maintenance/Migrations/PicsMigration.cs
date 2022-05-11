using Data.Implementation.DynamoDb;
using Data.Implementation.SQlite;

namespace Maintenance.Migrations;

public static class PicsMigration
{
    public static async Task SqliteToDynamo(DynamoPicsService dynamoService, SqlitePicsService sqliteService)
    {
        await ClearDynamo(dynamoService);

        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.InsertAsync(config);
        }
    }

    public static async Task DynamoToSqlite(DynamoPicsService dynamoService, SqlitePicsService sqliteService)
    {
        await ClearSqlite(sqliteService);
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.InsertAsync(config);
        }
    }

    private static async Task ClearDynamo(DynamoPicsService dynamoService)
    {
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.DeleteAsync(config.Id);
        }
    }

    private static async Task ClearSqlite(SqlitePicsService sqliteService)
    {
        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.DeleteAsync(config.Id);
        }
    }
}
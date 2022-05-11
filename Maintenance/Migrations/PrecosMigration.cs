using Data.Implementation.DynamoDb;
using Data.Implementation.SQlite;

namespace Maintenance.Migrations;

public static class PrecosMigration
{
    public static async Task SqliteToDynamo(DynamoPrecosService dynamoService, SqlitePrecosService sqliteService)
    {
        await ClearDynamo(dynamoService);

        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.InsertAsync(config);
        }
    }

    public static async Task DynamoToSqlite(DynamoPrecosService dynamoService, SqlitePrecosService sqliteService)
    {
        await ClearSqlite(sqliteService);
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.InsertAsync(config);
        }
    }

    private static async Task ClearDynamo(DynamoPrecosService dynamoService)
    {
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.DeleteAsync(config.Id);
        }
    }

    private static async Task ClearSqlite(SqlitePrecosService sqliteService)
    {
        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.DeleteAsync(config.Id);
        }
    }
}
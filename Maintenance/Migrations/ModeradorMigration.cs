using Data.Implementation.DynamoDb;
using Data.Implementation.SQlite;

namespace Maintenance.Migrations;

public static class ModeradorMigration
{
    public static async Task SqliteToDynamo(DynamoModeradorService dynamoService, SqliteModeradorService sqliteService)
    {
        await ClearDynamo(dynamoService);

        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.InsertAsync(config);
        }
    }

    public static async Task DynamoToSqlite(DynamoModeradorService dynamoService, SqliteModeradorService sqliteService)
    {
        await ClearSqlite(sqliteService);
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.InsertAsync(config);
        }
    }

    private static async Task ClearDynamo(DynamoModeradorService dynamoService)
    {
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.DeleteAsync(config.Id);
        }
    }

    private static async Task ClearSqlite(SqliteModeradorService sqliteService)
    {
        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.DeleteAsync(config.Id);
        }
    }
}
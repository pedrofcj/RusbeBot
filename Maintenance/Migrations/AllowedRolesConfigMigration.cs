using Data.Implementation.DynamoDb;
using Data.Implementation.SQlite;

namespace Maintenance.Migrations;

public static class AllowedRolesConfigMigration
{
    public static async Task SqliteToDynamo(DynamoAllowedRolesConfigService dynamoService, SqliteAllowedRolesConfigService sqliteService)
    {
        await ClearDynamo(dynamoService);

        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.InsertAsync(config);
        }
    }

    public static async Task DynamoToSqlite(DynamoAllowedRolesConfigService dynamoService, SqliteAllowedRolesConfigService sqliteService)
    {
        await ClearSqlite(sqliteService);
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.InsertAsync(config);
        }
    }

    private static async Task ClearDynamo(DynamoAllowedRolesConfigService dynamoService)
    {
        var configs = await dynamoService.GetAllAsync();
        foreach (var config in configs)
        {
            await dynamoService.DeleteAsync(config.Id);
        }
    }

    private static async Task ClearSqlite(SqliteAllowedRolesConfigService sqliteService)
    {
        var configs = await sqliteService.GetAllAsync();
        foreach (var config in configs)
        {
            await sqliteService.DeleteAsync(config.Id);
        }
    }
}
// See https://aka.ms/new-console-template for more information

using RusbeBot.Maintenance.Migrations;
using Microsoft.Extensions.Configuration;
using RusbeBot.Data.Implementation.DynamoDb;
using RusbeBot.Data.Implementation.SQlite;
using RusbeBot.Data.Values;

IConfigurationRoot configuration;

BuildConfiguration();
MigrateSqliteToDynamo();

Console.ReadKey();

void BuildConfiguration()
{
    var builder = new ConfigurationBuilder()        // Create a new instance of the config builder
        .SetBasePath(AppContext.BaseDirectory)      // Specify the default location for the config file
        .AddYamlFile("_config.yml");                // Add this (yaml encoded) file to the configuration
    try
    {
        configuration = builder.Build();
        AwsCredentials.SetCredentials(configuration["aws:accessKeyId"], configuration["aws:secretAccessKey"]);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

async void MigrateSqliteToDynamo()
{
    try
    {
        await AllowedChannelsConfigMigration.SqliteToDynamo(new DynamoAllowedChannelsConfigService(), new SqliteAllowedChannelsConfigService(configuration));
        await AllowedRolesConfigMigration.SqliteToDynamo(new DynamoAllowedRolesConfigService(), new SqliteAllowedRolesConfigService(configuration));
        await ModeradorMigration.SqliteToDynamo(new DynamoModeradorService(), new SqliteModeradorService(configuration));
        await PicsMigration.SqliteToDynamo(new DynamoPicsService(), new SqlitePicsService(configuration));
        await PrecosMigration.SqliteToDynamo(new DynamoPrecosService(), new SqlitePrecosService(configuration));

        await Task.Delay(1000);
        Console.WriteLine("Hello, World!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}
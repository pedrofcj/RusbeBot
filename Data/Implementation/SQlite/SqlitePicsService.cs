using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Implementation.SQlite;

public class SqlitePicsService : SqliteBaseService<Pics>, IPicsService
{
    public SqlitePicsService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }
}
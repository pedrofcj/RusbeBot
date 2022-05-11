using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Implementation.SQlite;

public class SqlitePicsService : SqliteBaseService<PicsModel>, IPicsService
{
    public SqlitePicsService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }
}
using Microsoft.Extensions.Configuration;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Implementation.SQlite;

public class SqlitePicsService : SqliteBaseService<PicsModel>, IPicsService
{
    public SqlitePicsService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }
}
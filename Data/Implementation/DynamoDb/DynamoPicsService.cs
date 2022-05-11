using Data.Interfaces;
using Data.Models;

namespace Data.Implementation.DynamoDb;

public class DynamoPicsService : DynamoBaseService<PicsModel>, IPicsService
{
}
using Amazon.DynamoDBv2.DataModel;
using SQLite;
using System;

namespace RusbeBot.Data.Models;

public class BaseModel
{
    [DynamoDBHashKey]
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty]
    public string GuildId { get; set; }


}
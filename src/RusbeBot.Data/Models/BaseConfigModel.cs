using Amazon.DynamoDBv2.DataModel;

namespace RusbeBot.Data.Models;

public class BaseConfigModel : BaseModel
{
    [DynamoDBProperty]
    public string CommandName { get; set; }

}
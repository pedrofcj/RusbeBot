using Amazon.DynamoDBv2.DataModel;

namespace Data.Models;

public class BaseConfigModel : BaseModel
{
    [DynamoDBProperty]
    public string CommandName { get; set; }

}
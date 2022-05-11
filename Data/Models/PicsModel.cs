using Amazon.DynamoDBv2.DataModel;

namespace Data.Models;

[DynamoDBTable("Pics")]
[SQLite.Table("Pics")]
public class PicsModel : BaseModel
{
    [DynamoDBProperty] public string Category { get; set; }

    [DynamoDBProperty] public string Url { get; set; }

}
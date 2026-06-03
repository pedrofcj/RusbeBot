using Amazon.DynamoDBv2.DataModel;

namespace RusbeBot.Data.Models;

[DynamoDBTable("Moderador")]
[SQLite.Table("Moderador")]
public class ModeradorModel : BaseModel
{
    [DynamoDBProperty]
    public string UserId { get; set; }

}
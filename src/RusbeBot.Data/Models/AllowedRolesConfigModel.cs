using Amazon.DynamoDBv2.DataModel;

namespace RusbeBot.Data.Models;

[DynamoDBTable("AllowedRolesConfig")]
[SQLite.Table("AllowedRolesConfig")]
public class AllowedRolesConfigModel : BaseConfigModel
{
    [DynamoDBProperty]
    public string RoleId { get; set; }

}
using Amazon.DynamoDBv2.DataModel;

namespace RusbeBot.Data.Models;

[DynamoDBTable("AllowedChannelsConfig")]
[SQLite.Table("AllowedChannelsConfig")]
public class AllowedChannelsConfigModel : BaseConfigModel
{
    [DynamoDBProperty]
    public string ChannelId { get; set; }

}
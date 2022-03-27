using Amazon.DynamoDBv2.DataModel;

namespace Data.Models
{
    [DynamoDBTable("AllowedChannelsConfig")]
    [SQLite.Table("AllowedChannelsConfig")]
    public class AllowedChannelsConfigModel : BaseConfigModel
    {
        public string ChannelId { get; set; }

    }
}
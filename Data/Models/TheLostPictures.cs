using Amazon.DynamoDBv2.DataModel;

namespace Data.Models
{

    [DynamoDBTable("TheLostPics")]
    public class TheLostPictures : BaseModel
    {
        [DynamoDBProperty] public string Category { get; set; }

        [DynamoDBProperty] public string Url { get; set; }

    }
}
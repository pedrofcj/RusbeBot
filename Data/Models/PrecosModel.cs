using System;
using Amazon.DynamoDBv2.DataModel;

namespace Data.Models
{
    [DynamoDBTable("Precos")]
    public class PrecosModel : BaseModel
    {
        [DynamoDBProperty] public DateTime Data { get; set; }
        
        [DynamoDBProperty] public int MunicaoP { get; set; }
        [DynamoDBProperty] public int MunicaoM { get; set; }
        [DynamoDBProperty] public int MunicaoG { get; set; }
        
        [DynamoDBProperty] public int PistolaP { get; set; }
        [DynamoDBProperty] public int PistolaM { get; set; }
        [DynamoDBProperty] public int PistolaG { get; set; }
        
        [DynamoDBProperty] public int SMGP { get; set; }
        [DynamoDBProperty] public int SMGM { get; set; }
        [DynamoDBProperty] public int SMGG { get; set; }
        
        [DynamoDBProperty] public int RifleP { get; set; }
        [DynamoDBProperty] public int RifleM { get; set; }
        [DynamoDBProperty] public int RifleG { get; set; }
    }
}
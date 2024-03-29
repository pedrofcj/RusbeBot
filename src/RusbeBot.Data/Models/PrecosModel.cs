﻿using Amazon.DynamoDBv2.DataModel;
using System;

namespace RusbeBot.Data.Models;

[DynamoDBTable("Precos")]
[SQLite.Table("Precos")]
public class PrecosModel : BaseModel
{
    [DynamoDBProperty] public DateTime Data { get; set; }
    

    [DynamoDBProperty] public int KitReparosP { get; set; }
    [DynamoDBProperty] public int KitReparosM { get; set; }
    [DynamoDBProperty] public int KitReparosG { get; set; }
    
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
    
    [DynamoDBProperty] public int KitReparosPMarcado { get; set; }
    [DynamoDBProperty] public int KitReparosMMarcado { get; set; }
    [DynamoDBProperty] public int KitReparosGMarcado { get; set; }

    [DynamoDBProperty] public int MunicaoPMarcado { get; set; }
    [DynamoDBProperty] public int MunicaoMMarcado { get; set; }
    [DynamoDBProperty] public int MunicaoGMarcado { get; set; }

    [DynamoDBProperty] public int PistolaPMarcado { get; set; }
    [DynamoDBProperty] public int PistolaMMarcado { get; set; }
    [DynamoDBProperty] public int PistolaGMarcado { get; set; }

    [DynamoDBProperty] public int SMGPMarcado { get; set; }
    [DynamoDBProperty] public int SMGMMarcado { get; set; }
    [DynamoDBProperty] public int SMGGMarcado { get; set; }

    [DynamoDBProperty] public int RiflePMarcado { get; set; }
    [DynamoDBProperty] public int RifleMMarcado { get; set; }
    [DynamoDBProperty] public int RifleGMarcado { get; set; }
}
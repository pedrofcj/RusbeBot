﻿using Amazon.DynamoDBv2.DataModel;

namespace Data.Models
{
    [DynamoDBTable("AllowedRolesConfig")]
    [SQLite.Table("AllowedRolesConfig")]
    public class AllowedRolesConfigModel : BaseConfigModel
    {
        public string RoleId { get; set; }

    }
}
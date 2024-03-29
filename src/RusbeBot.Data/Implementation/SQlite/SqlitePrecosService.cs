﻿using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Data.Implementation.SQlite;

public class SqlitePrecosService : SqliteBaseService<PrecosModel>, IPrecosService
{
    public SqlitePrecosService(IConfigurationRoot configurationRoot) : base(configurationRoot)
    {
    }

    public async Task<PrecosModel> GetByDate(DateTime date, string guildId)
    {
        var result = await Db.FindAsync<PrecosModel>(model => model.Data == date && model.GuildId == guildId);
        return result;
    }
}
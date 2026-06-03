using System.Globalization;
using Discord.WebSocket;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Core.Extensions;

public static class CheckPriceExtension
{

    public static async Task CheckPrice(this SocketUserMessage message, IPrecosService precosService)
    {
        if (message.Channel is not SocketGuildChannel { Id: (877332271764504636 or 934125922284634232 or 914914042966069258) } channel) return;

        var str = CleanString(message.Content);
        var parts = str.Split(' ');

        if (!DateTime.TryParseExact(parts[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)) return;

        var values = new List<int>();
        foreach (var part in parts)
        {
            if (int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                values.Add(value);
            }
        }

        if (values.Count != 30)
        {
            await message.Channel.SendMessageAsync("Não foi possível extrair todos os valores, por favor tente enviar novamente.");
            return;
        }

        var preco = new PrecosModel
        {
            Data = date,
            GuildId = channel.Guild.Id.ToString(),
            
            KitReparosP = values[0],
            KitReparosM = values[1],
            KitReparosG = values[2], 

            MunicaoP = values[3],
            MunicaoM = values[4],
            MunicaoG = values[5],

            PistolaP = values[6],
            PistolaM = values[7],
            PistolaG = values[8],

            SMGP = values[9],
            SMGM = values[10],
            SMGG = values[11],

            RifleP = values[12],
            RifleM = values[13],
            RifleG = values[14],
            
            KitReparosPMarcado = values[15],
            KitReparosMMarcado = values[16],
            KitReparosGMarcado = values[17],

            MunicaoPMarcado = values[18],
            MunicaoMMarcado = values[19],
            MunicaoGMarcado = values[20],

            PistolaPMarcado = values[21],
            PistolaMMarcado = values[22],
            PistolaGMarcado = values[23],

            SMGPMarcado = values[24],
            SMGMMarcado = values[25],
            SMGGMarcado = values[26],

            RiflePMarcado = values[27],
            RifleMMarcado = values[28],
            RifleGMarcado = values[29],
        };

        var existente = await precosService.GetByDate(preco.Data, channel.Guild.Id.ToString());
        if (existente != null)
        {
            await message.Channel.SendMessageAsync("Já existe um preço para esta data.");
            return;
        }

        var id = await precosService.InsertAsync(preco);

        if (string.IsNullOrEmpty(id))
        {
            await message.Channel.SendMessageAsync("Não foi possível adicionar o preço automaticamente.");
            return;
        }

        await message.Channel.SendMessageAsync("O preço foi adicionado automaticamente!");
    }

    private static string CleanString(string str)
    {
        while (str.Contains("."))
        {
            str = str.Replace(".", "");
        }

        while (str.Contains("\n"))
        {
            str = str.Replace("\n", " ");
        }

        while (str.Contains("  "))
        {
            str = str.Replace("  ", " ");
        }

        while (str.Contains("|"))
        {
            str = str.Replace("|", "");
        }

        return str;
    }
}
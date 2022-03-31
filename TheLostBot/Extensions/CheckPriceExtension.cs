using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Discord.WebSocket;

namespace TheLostBot.Extensions
{
    public static class CheckPriceExtension
    {

        public static async Task CheckPrice(this SocketUserMessage message, IPrecosService precosService)
        {
            if (message.Channel is not SocketGuildChannel {Id: (877332271764504636 or 934125922284634232 or 914914042966069258)} channel) return;

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

            if (values.Count != 24)
            {
                await message.Channel.SendMessageAsync("Não foi possível extrair todos os valores, por favor tente enviar novamente.");
                return;
            }

            var preco = new PrecosModel
            {
                Data = date,
                GuildId = channel.Guild.Id.ToString(),

                MunicaoP = values[0],
                MunicaoM = values[1],
                MunicaoG = values[2],

                PistolaP = values[3],
                PistolaM = values[4],
                PistolaG = values[5],

                SMGP = values[6],
                SMGM = values[7],
                SMGG = values[8],

                RifleP = values[9],
                RifleM = values[10],
                RifleG = values[11],

                MunicaoPMarcado = values[12],
                MunicaoMMarcado = values[13],
                MunicaoGMarcado = values[14],

                PistolaPMarcado = values[15],
                PistolaMMarcado = values[16],
                PistolaGMarcado = values[17],

                SMGPMarcado = values[18],
                SMGMMarcado = values[19],
                SMGGMarcado = values[20],

                RiflePMarcado = values[21],
                RifleMMarcado = values[22],
                RifleGMarcado = values[23],
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
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Discord.Commands;
using TheLostBot.Attributes;
using TheLostBot.Values.TheLost;

namespace TheLostBot.Modules
{
    [RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado no Discord do The Lost")]
    public class McModule : ModuleBase<SocketCommandContext>
    {

        private readonly IPrecosService _precosService;
        public McModule(IPrecosService precosService) => _precosService = precosService;

        #region Preços

        [Command("precos")]
        [RequiredRoles(TheLostRolesEnum.Member)]
        [AllowedChannels(TheLostChannelsEnum.RhCotacaoArmas, TheLostChannelsEnum.ConversasPrivadoComandos)]
        public async Task PrecosAsync()
        {

            var allPrecos = await _precosService.GetAllAsync();
            var precosList = allPrecos.Where(d => d.GuildId == Context.Guild.Id.ToString()).ToList();
            var precos = precosList.OrderByDescending(d => d.Data).FirstOrDefault();

            if (precos == null)
            {
                await ReplyAsync("Ocorreu um erro ao buscar os preços");
                return;
            }

            var media = GetAvg(precosList);

            var response = new StringBuilder();

            response.AppendLine($"Data: {precos.Data.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}");

            // Munição
            response.AppendLine(media.MunicaoP > precos.MunicaoP
                ? $"+ Munição P: {precos.MunicaoP:C} ({Convert.ToInt32(precos.MunicaoP - media.MunicaoP):C}) {CalculatePercentage(precos.MunicaoP, media.MunicaoP)}"
                : $"- Munição P: {precos.MunicaoP:C} ({Convert.ToInt32(precos.MunicaoP - media.MunicaoP):C}) {CalculatePercentage(precos.MunicaoP, media.MunicaoP)}");

            response.AppendLine(media.MunicaoM > precos.MunicaoM
                ? $"+ Munição M: {precos.MunicaoM:C} ({Convert.ToInt32(precos.MunicaoM - media.MunicaoM):C}) {CalculatePercentage(precos.MunicaoM, media.MunicaoM)}"
                : $"- Munição M: {precos.MunicaoM:C} ({Convert.ToInt32(precos.MunicaoM - media.MunicaoM):C}) {CalculatePercentage(precos.MunicaoM, media.MunicaoM)}");

            response.AppendLine(media.MunicaoG > precos.MunicaoG
                ? $"+ Munição G: {precos.MunicaoG:C} ({Convert.ToInt32(precos.MunicaoG - media.MunicaoG):C}) {CalculatePercentage(precos.MunicaoG, media.MunicaoG)}"
                : $"- Munição G: {precos.MunicaoG:C} ({Convert.ToInt32(precos.MunicaoG - media.MunicaoG):C}) {CalculatePercentage(precos.MunicaoG, media.MunicaoG)}");

            response.AppendLine();

            // Pistola
            response.AppendLine(media.PistolaP > precos.PistolaP
                ? $"+ Pistola P: {precos.PistolaP:C} ({Convert.ToInt32(precos.PistolaP - media.PistolaP):C}) {CalculatePercentage(precos.PistolaP, media.PistolaP)}"
                : $"- Pistola P: {precos.PistolaP:C} ({Convert.ToInt32(precos.PistolaP - media.PistolaP):C}) {CalculatePercentage(precos.PistolaP, media.PistolaP)}");

            response.AppendLine(media.PistolaM > precos.PistolaM
                ? $"+ Pistola M: {precos.PistolaM:C} ({Convert.ToInt32(precos.PistolaM - media.PistolaM):C}) {CalculatePercentage(precos.PistolaM, media.PistolaM)}"
                : $"- Pistola M: {precos.PistolaM:C} ({Convert.ToInt32(precos.PistolaM - media.PistolaM):C}) {CalculatePercentage(precos.PistolaM, media.PistolaM)}");

            response.AppendLine(media.PistolaG > precos.PistolaG
                ? $"+ Pistola G: {precos.PistolaG:C} ({Convert.ToInt32(precos.PistolaG - media.PistolaG):C}) {CalculatePercentage(precos.PistolaG, media.PistolaG)}"
                : $"- Pistola G: {precos.PistolaG:C} ({Convert.ToInt32(precos.PistolaG - media.PistolaG):C}) {CalculatePercentage(precos.PistolaG, media.PistolaG)}");

            response.AppendLine();

            // SMG
            response.AppendLine(media.SMGP > precos.SMGP
                ? $"+ SMG P: {precos.SMGP:C} ({Convert.ToInt32(precos.SMGP - media.SMGP):C}) {CalculatePercentage(precos.SMGP, media.SMGP)}"
                : $"- SMG P: {precos.SMGP:C} ({Convert.ToInt32(precos.SMGP - media.SMGP):C}) {CalculatePercentage(precos.SMGP, media.SMGP)}");

            response.AppendLine(media.SMGM > precos.SMGM
                ? $"+ SMG M: {precos.SMGM:C} ({Convert.ToInt32(precos.SMGM - media.SMGM):C}) {CalculatePercentage(precos.SMGM, media.SMGM)}"
                : $"- SMG M: {precos.SMGM:C} ({Convert.ToInt32(precos.SMGM - media.SMGM):C}) {CalculatePercentage(precos.SMGM, media.SMGM)}");

            response.AppendLine(media.SMGG > precos.SMGG
                ? $"+ SMG G: {precos.SMGG:C} ({Convert.ToInt32(precos.SMGG - media.SMGG):C}) {CalculatePercentage(precos.SMGG, media.SMGG)}"
                : $"- SMG G: {precos.SMGG:C} ({Convert.ToInt32(precos.SMGG - media.SMGG):C}) {CalculatePercentage(precos.SMGG, media.SMGG)}");

            response.AppendLine();

            // Rifle
            response.AppendLine(media.RifleP > precos.RifleP
                ? $"+ Rifle P: {precos.RifleP:C} ({Convert.ToInt32(precos.RifleP - media.RifleP):C}) {CalculatePercentage(precos.RifleP, media.RifleP)}"
                : $"- Rifle P: {precos.RifleP:C} ({Convert.ToInt32(precos.RifleP - media.RifleP):C}) {CalculatePercentage(precos.RifleP, media.RifleP)}");

            response.AppendLine(media.RifleM > precos.RifleM
                ? $"+ Rifle M: {precos.RifleM:C} ({Convert.ToInt32(precos.RifleM - media.RifleM):C}) {CalculatePercentage(precos.RifleM, media.RifleM)}"
                : $"- Rifle M: {precos.RifleM:C} ({Convert.ToInt32(precos.RifleM - media.RifleM):C}) {CalculatePercentage(precos.RifleM, media.RifleM)}");

            response.AppendLine(media.RifleG > precos.RifleG
                ? $"+ Rifle G: {precos.RifleG:C} ({Convert.ToInt32(precos.RifleG - media.RifleG):C}) {CalculatePercentage(precos.RifleG, media.RifleG)}"
                : $"- Rifle G: {precos.RifleG:C} ({Convert.ToInt32(precos.RifleG - media.RifleG):C}) {CalculatePercentage(precos.RifleG, media.RifleG)}");


            while (response.ToString().Contains("(("))
            {
                response = response.Replace("((", "(");
            }

            while (response.ToString().Contains("))"))
            {
                response = response.Replace("))", ")");
            }

            await ReplyAsync($"```diff" + Environment.NewLine +
                             $"{response}" + Environment.NewLine +
                             $"```");
        }

        private string CalculatePercentage(int price, double avg)
        {
            var response = "";
            var percentage = Math.Round(avg / price * 100 - 100, 2);
            percentage *= -1;

            if (percentage > 0) response += "+";

            response += $"{percentage.ToString(CultureInfo.InvariantCulture)}%";
            return response;
        }

        [Command("addpreco")]
        [RequiredRoles(TheLostRolesEnum.Conselho)]
        public async Task AddPrecoAsync([Remainder] string text)
        {
            while (text.Contains("  ")) text = text.Replace("  ", " ");
            text = text.Replace(".", "");

            var args = text.Split(' ');

            if (args.Length != 13)
            {
                await ReplyAsync("Número de argumentos inválidos");
                return;
            }

            var newPreco = new PrecosModel
            {
                GuildId = Context.Guild.Id.ToString(),
                Data = DateTime.ParseExact(args[0], "dd/MM/yyyy", CultureInfo.InvariantCulture),

                MunicaoP = Convert.ToInt32(args[1]),
                MunicaoM = Convert.ToInt32(args[2]),
                MunicaoG = Convert.ToInt32(args[3]),

                PistolaP = Convert.ToInt32(args[4]),
                PistolaM = Convert.ToInt32(args[5]),
                PistolaG = Convert.ToInt32(args[6]),

                SMGP = Convert.ToInt32(args[7]),
                SMGM = Convert.ToInt32(args[8]),
                SMGG = Convert.ToInt32(args[9]),

                RifleP = Convert.ToInt32(args[10]),
                RifleM = Convert.ToInt32(args[11]),
                RifleG = Convert.ToInt32(args[12])
            };

            var existente = await _precosService.GetByDate(newPreco.Data);

            if (existente != null)
            {
                await ReplyAsync("Já existe um preço para esta data.");
                return;
            }

            await _precosService.InsertAsync(newPreco);

            var msg = await ReplyAsync("Preços inseridos!");
            await Task.Delay(3000);
            await msg.DeleteAsync();
        }

        [Command("delpreco")]
        [RequiredRoles(TheLostRolesEnum.Conselho)]
        public async Task DelPrecoAsync([Remainder] string text)
        {
            var date = DateTime.ParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var existente = await _precosService.GetByDate(date);

            if (existente == null)
            {
                await ReplyAsync("Não foi encontrado uma cotação para essa data");
                return;
            }

            await _precosService.DeleteAsync(existente.Id);

            var msg = await ReplyAsync("Cotação removida!");
            await Task.Delay(3000);
            await msg.DeleteAsync();
        }

        private PrecoRelatorioModel GetAvg(List<PrecosModel> precos)
        {
            var avg = new PrecoRelatorioModel
            {
                MunicaoP = precos.Average(d => d.MunicaoP),
                MunicaoM = precos.Average(d => d.MunicaoM),
                MunicaoG = precos.Average(d => d.MunicaoG),

                PistolaP = precos.Average(d => d.PistolaP),
                PistolaM = precos.Average(d => d.PistolaM),
                PistolaG = precos.Average(d => d.PistolaG),

                SMGP = precos.Average(d => d.SMGP),
                SMGM = precos.Average(d => d.SMGM),
                SMGG = precos.Average(d => d.SMGG),

                RifleP = precos.Average(d => d.RifleP),
                RifleM = precos.Average(d => d.RifleM),
                RifleG = precos.Average(d => d.RifleG)

            };

            return avg;
        }

        #endregion

        #region IPVA

        private static readonly DateTime ReferenceDate = new DateTime(2022, 03, 07);

        [Command("ipva")]
        public async Task IpvaAsync()
        {

            var proximoIpva = ReferenceDate;
            var hoje = DateTime.ParseExact(DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            while (proximoIpva <= hoje)
            {
                proximoIpva = proximoIpva.AddDays(12);
                if (proximoIpva == hoje)
                {
                    await ReplyAsync("Hoje é dia de pagar IPVA!");
                }
            }

            await ReplyAsync($"Próximo IPVA será dia {proximoIpva.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}");
        }

        #endregion

        #region Cargas

        private readonly int _cargaPMin = 15;
        private readonly int _cargaPMax = 20;

        private readonly int _cargaMMin = 25;
        private readonly int _cargaMMax = 35;

        private readonly int _cargaGMin = 40;
        private readonly int _cargaGMax = 55;

        private readonly int _pesoMun = 3;
        private readonly int _pesoPT = 6;
        private readonly int _pesoSmg = 25;
        private readonly int _pesoRifle = 40;

        [Command("cargas")]
        [RequiredRoles(TheLostRolesEnum.Member)]
        [AllowedChannels(TheLostChannelsEnum.ConversasPrivadoChatPrivado, TheLostChannelsEnum.ConversasPrivadoComandos)]
        public async Task CargasAsync()
        {
            var response = new StringBuilder();

            response.AppendLine("+ Munição: ");
            response.AppendLine($"- Pequeno: {_cargaPMin}-{_cargaPMax} caixas. Peso: {_cargaPMin * _pesoMun}-{_cargaPMax * _pesoMun} kgs.");
            response.AppendLine($"- Médio: {_cargaMMin}-{_cargaMMax} caixas. Peso: {_cargaMMin * _pesoMun}-{_cargaMMax * _pesoMun} kgs.");
            response.AppendLine($"- Grande: {_cargaGMin}-{_cargaGMax} caixas. Peso: {_cargaGMin * _pesoMun}-{_cargaGMax * _pesoMun} kgs.");

            response.AppendLine();

            response.AppendLine("+ Pistola: ");
            response.AppendLine($"- Pequeno: {_cargaPMin}-{_cargaPMax} caixas. Peso: {_cargaPMin * _pesoPT}-{_cargaPMax * _pesoPT} kgs.");
            response.AppendLine($"- Médio: {_cargaMMin}-{_cargaMMax} caixas. Peso: {_cargaMMin * _pesoPT}-{_cargaMMax * _pesoPT} kgs.");
            response.AppendLine($"- Grande: {_cargaGMin}-{_cargaGMax} caixas. Peso: {_cargaGMin * _pesoPT}-{_cargaGMax * _pesoPT} kgs.");

            response.AppendLine();

            response.AppendLine("+ SMG: ");
            response.AppendLine($"- Pequeno: {_cargaPMin}-{_cargaPMax} caixas. Peso: {_cargaPMin * _pesoSmg}-{_cargaPMax * _pesoSmg} kgs.");
            response.AppendLine($"- Médio: {_cargaMMin}-{_cargaMMax} caixas. Peso: {_cargaMMin * _pesoSmg}-{_cargaMMax * _pesoSmg} kgs.");
            response.AppendLine($"- Grande: {_cargaGMin}-{_cargaGMax} caixas. Peso: {_cargaGMin * _pesoSmg}-{_cargaGMax * _pesoSmg} kgs.");

            response.AppendLine();

            response.AppendLine("+ Rifle: ");
            response.AppendLine($"- Pequeno: {_cargaPMin}-{_cargaPMax} caixas. Peso: {_cargaPMin * _pesoRifle}-{_cargaPMax * _pesoRifle} kgs.");
            response.AppendLine($"- Médio: {_cargaMMin}-{_cargaMMax} caixas. Peso: {_cargaMMin * _pesoRifle}-{_cargaMMax * _pesoRifle} kgs.");
            response.AppendLine($"- Grande: {_cargaGMin}-{_cargaGMax} caixas. Peso: {_cargaGMin * _pesoRifle}-{_cargaGMax * _pesoRifle} kgs.");

            while (response.ToString().Contains("(("))
            {
                response = response.Replace("((", "(");
            }

            while (response.ToString().Contains("))"))
            {
                response = response.Replace("))", ")");
            }

            await ReplyAsync($"```diff" + Environment.NewLine +
                             $"{response}" + Environment.NewLine +
                             $"```");
        }

        #endregion

    }
}
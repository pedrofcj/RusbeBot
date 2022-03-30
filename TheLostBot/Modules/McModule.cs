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
    [CommandValidation(true, true)]
    [RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]

    public class McModule : ModuleBase<SocketCommandContext>
    {

        private readonly IPrecosService _precosService;
        public McModule(IPrecosService precosService) => _precosService = precosService;

        #region Preços

        [Command("precos")]
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
            response.AppendLine(FormatValue("Munição P", precos.MunicaoP, media.MunicaoP));
            response.AppendLine(FormatValue("Munição M", precos.MunicaoM, media.MunicaoM));
            response.AppendLine(FormatValue("Munição G", precos.MunicaoG, media.MunicaoG));
            response.AppendLine();

            // Pistola
            response.AppendLine(FormatValue("Pistola P", precos.PistolaP, media.PistolaP));
            response.AppendLine(FormatValue("Pistola M", precos.PistolaM, media.PistolaM));
            response.AppendLine(FormatValue("Pistola G", precos.PistolaG, media.PistolaG));
            response.AppendLine();

            // SMG
            response.AppendLine(FormatValue("SMG P", precos.SMGP, media.SMGP));
            response.AppendLine(FormatValue("SMG M", precos.SMGM, media.SMGM));
            response.AppendLine(FormatValue("SMG G", precos.SMGG, media.SMGG));
            response.AppendLine();

            // Rifle
            response.AppendLine(FormatValue("Rifle P", precos.RifleP, media.RifleP));
            response.AppendLine(FormatValue("Rifle M", precos.RifleM, media.RifleM));
            response.AppendLine(FormatValue("Rifle G", precos.RifleG, media.RifleG));
            response.AppendLine();

            response.AppendLine("DINHEIRO MARCADO");

            // Munição (dinheiro marcado)
            response.AppendLine(FormatValue("Munição P", precos.MunicaoPMarcado, media.MunicaoPMarcado));
            response.AppendLine(FormatValue("Munição M", precos.MunicaoMMarcado, media.MunicaoMMarcado));
            response.AppendLine(FormatValue("Munição G", precos.MunicaoGMarcado, media.MunicaoGMarcado));
            response.AppendLine();

            // Pistola (dinheiro marcado)
            response.AppendLine(FormatValue("Pistola P", precos.PistolaPMarcado, media.PistolaPMarcado));
            response.AppendLine(FormatValue("Pistola M", precos.PistolaMMarcado, media.PistolaMMarcado));
            response.AppendLine(FormatValue("Pistola G", precos.PistolaGMarcado, media.PistolaGMarcado));
            response.AppendLine();

            // SMG (dinheiro marcado)
            response.AppendLine(FormatValue("SMG P", precos.SMGPMarcado, media.SMGPMarcado));
            response.AppendLine(FormatValue("SMG M", precos.SMGMMarcado, media.SMGMMarcado));
            response.AppendLine(FormatValue("SMG G", precos.SMGGMarcado, media.SMGGMarcado));
            response.AppendLine();

            // Rifle (dinheiro marcado)
            response.AppendLine(FormatValue("Rifle P", precos.RiflePMarcado, media.RiflePMarcado));
            response.AppendLine(FormatValue("Rifle M", precos.RifleMMarcado, media.RifleMMarcado));
            response.AppendLine(FormatValue("Rifle G", precos.RifleGMarcado, media.RifleGMarcado));

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

        private string FormatValue(string tipo, int preco, double media)
        {
            return media > preco
                ? $"+ {tipo}: {preco:C} ({Convert.ToInt32(preco - media):C}) {CalculatePercentage(preco, media)}"
                : $"- {tipo}: {preco:C} ({Convert.ToInt32(preco - media):C}) {CalculatePercentage(preco, media)}";
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
                MunicaoP = precos.Where(d => d.MunicaoP > 0).Average(d => d.MunicaoP),
                MunicaoM = precos.Where(d => d.MunicaoM > 0).Average(d => d.MunicaoM),
                MunicaoG = precos.Where(d => d.MunicaoG > 0).Average(d => d.MunicaoG),

                PistolaP = precos.Where(d => d.PistolaP > 0).Average(d => d.PistolaP),
                PistolaM = precos.Where(d => d.PistolaM > 0).Average(d => d.PistolaM),
                PistolaG = precos.Where(d => d.PistolaG > 0).Average(d => d.PistolaG),

                SMGP = precos.Where(d => d.SMGP > 0).Average(d => d.SMGP),
                SMGM = precos.Where(d => d.SMGM > 0).Average(d => d.SMGM),
                SMGG = precos.Where(d => d.SMGG > 0).Average(d => d.SMGG),

                RifleP = precos.Where(d => d.RifleP > 0).Average(d => d.RifleP),
                RifleM = precos.Where(d => d.RifleM > 0).Average(d => d.RifleM),
                RifleG = precos.Where(d => d.RifleG > 0).Average(d => d.RifleG),

                MunicaoPMarcado = precos.Where(d => d.MunicaoPMarcado > 0).Average(d => d.MunicaoPMarcado),
                MunicaoMMarcado = precos.Where(d => d.MunicaoMMarcado > 0).Average(d => d.MunicaoMMarcado),
                MunicaoGMarcado = precos.Where(d => d.MunicaoGMarcado > 0).Average(d => d.MunicaoGMarcado),

                PistolaPMarcado = precos.Where(d => d.PistolaPMarcado > 0).Average(d => d.PistolaPMarcado),
                PistolaMMarcado = precos.Where(d => d.PistolaMMarcado > 0).Average(d => d.PistolaMMarcado),
                PistolaGMarcado = precos.Where(d => d.PistolaGMarcado > 0).Average(d => d.PistolaGMarcado),

                SMGPMarcado = precos.Where(d => d.SMGPMarcado > 0).Average(d => d.SMGPMarcado),
                SMGMMarcado = precos.Where(d => d.SMGMMarcado > 0).Average(d => d.SMGMMarcado),
                SMGGMarcado = precos.Where(d => d.SMGGMarcado > 0).Average(d => d.SMGGMarcado),

                RiflePMarcado = precos.Where(d => d.RiflePMarcado > 0).Average(d => d.RiflePMarcado),
                RifleMMarcado = precos.Where(d => d.RifleMMarcado > 0).Average(d => d.RifleMMarcado),
                RifleGMarcado = precos.Where(d => d.RifleGMarcado > 0).Average(d => d.RifleGMarcado)

            };

            return avg;
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
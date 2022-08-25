using System.Globalization;
using System.Text;
using Discord.Commands;
using RusbeBot.Core.Attributes;
using RusbeBot.Core.Helpers;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Core.Modules.TextCommands;

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
            const string errorMessage = "Ocorreu um erro ao buscar os preços";
            SentryHelper.Log($"{errorMessage}{Environment.NewLine}Objeto de preços era null.", Context);
            await ReplyAsync(errorMessage);
            return;
        }

        var media = GetAvg(precosList);

        var response = new StringBuilder();

        response.AppendLine($"RusbeBot.Data: {precos.Data.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}");
        
        // Kit
        response.AppendLine(FormatValue("Kit P", precos.KitReparosP, media.KitReparosP));
        response.AppendLine(FormatValue("Kit M", precos.KitReparosM, media.KitReparosM));
        response.AppendLine(FormatValue("Kit G", precos.KitReparosG, media.KitReparosG));
        response.AppendLine();

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
        
        // Kit (dinheiro marcado)
        response.AppendLine(FormatValue("Kit P", precos.KitReparosPMarcado, media.KitReparosPMarcado));
        response.AppendLine(FormatValue("Kit M", precos.KitReparosMMarcado, media.KitReparosMMarcado));
        response.AppendLine(FormatValue("Kit G", precos.KitReparosGMarcado, media.KitReparosGMarcado));
        response.AppendLine();

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

        var existente = await _precosService.GetByDate(newPreco.Data, Context.Guild.Id.ToString());

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

        var existente = await _precosService.GetByDate(date, Context.Guild.Id.ToString());

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
            KitReparosP = precos.Where(d => d.KitReparosP > 0).Average(d => d.KitReparosP),
            KitReparosM = precos.Where(d => d.KitReparosM > 0).Average(d => d.KitReparosM),
            KitReparosG = precos.Where(d => d.KitReparosG > 0).Average(d => d.KitReparosG),
            
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

            KitReparosPMarcado = precos.Where(d => d.KitReparosPMarcado > 0).Average(d => d.KitReparosPMarcado),
            KitReparosMMarcado = precos.Where(d => d.KitReparosMMarcado > 0).Average(d => d.KitReparosMMarcado),
            KitReparosGMarcado = precos.Where(d => d.KitReparosGMarcado > 0).Average(d => d.KitReparosGMarcado),
            
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
    
    private const int CargaKitMunPMin = 15;
    private const int CargaKitMunPMax = 20;
    
    private const int CargaKitMunMMin = 25;
    private const int CargaKitMunMMax = 35;
    
    private const int CargaKitMunGMin = 40;
    private const int CargaKitMunGMax = 55;

    private const int CargaArmasPMin = 3;
    private const int CargaArmasPMax = 5;

    private const int CargaArmasMMin = 8;
    private const int CargaArmasMMax = 12;

    private const int CargaArmasGMin = 25;
    private const int CargaArmasGMax = 30;

    private const int PesoKit = 3;
    private const int PesoMun = 3;
    private const int PesoPT = 6;
    private const int PesoSmg = 25;
    private const int PesoRifle = 40;

    [Command("cargas")]
    public async Task CargasAsync()
    {
        var response = new StringBuilder();
        
        response.AppendLine("+ Kits: ");
        response.AppendLine($"- Pequeno: {CargaKitMunPMin}-{CargaKitMunPMax} caixas. Peso: {CargaKitMunPMin * PesoKit}-{CargaKitMunPMax * PesoKit} kgs.");
        response.AppendLine($"- Médio: {CargaKitMunMMin}-{CargaKitMunMMax} caixas. Peso: {CargaKitMunMMin * PesoKit}-{CargaKitMunMMax * PesoKit} kgs.");
        response.AppendLine($"- Grande: {CargaKitMunGMin}-{CargaKitMunGMax} caixas. Peso: {CargaKitMunGMin * PesoKit}-{CargaKitMunGMax * PesoKit} kgs.");

        response.AppendLine();

        response.AppendLine("+ Munição: ");
        response.AppendLine($"- Pequeno: {CargaKitMunPMin}-{CargaKitMunPMax} caixas. Peso: {CargaKitMunPMin * PesoMun}-{CargaKitMunPMax * PesoMun} kgs.");
        response.AppendLine($"- Médio: {CargaKitMunMMin}-{CargaKitMunMMax} caixas. Peso: {CargaKitMunMMin * PesoMun}-{CargaKitMunMMax * PesoMun} kgs.");
        response.AppendLine($"- Grande: {CargaKitMunGMin}-{CargaKitMunGMax} caixas. Peso: {CargaKitMunGMin * PesoMun}-{CargaKitMunGMax * PesoMun} kgs.");

        response.AppendLine();

        response.AppendLine("+ Pistola: ");
        response.AppendLine($"- Pequeno: {CargaArmasPMin}-{CargaArmasPMax} caixas. Peso: {CargaArmasPMin * PesoPT}-{CargaArmasPMax * PesoPT} kgs.");
        response.AppendLine($"- Médio: {CargaArmasMMin}-{CargaArmasMMax} caixas. Peso: {CargaArmasMMin * PesoPT}-{CargaArmasMMax * PesoPT} kgs.");
        response.AppendLine($"- Grande: {CargaArmasGMin}-{CargaArmasGMax} caixas. Peso: {CargaArmasGMin * PesoPT}-{CargaArmasGMax * PesoPT} kgs.");

        response.AppendLine();

        response.AppendLine("+ SMG: ");
        response.AppendLine($"- Pequeno: {CargaArmasPMin}-{CargaArmasPMax} caixas. Peso: {CargaArmasPMin * PesoSmg}-{CargaArmasPMax * PesoSmg} kgs.");
        response.AppendLine($"- Médio: {CargaArmasMMin}-{CargaArmasMMax} caixas. Peso: {CargaArmasMMin * PesoSmg}-{CargaArmasMMax * PesoSmg} kgs.");
        response.AppendLine($"- Grande: {CargaArmasGMin}-{CargaArmasGMax} caixas. Peso: {CargaArmasGMin * PesoSmg}-{CargaArmasGMax * PesoSmg} kgs.");

        response.AppendLine();

        response.AppendLine("+ Rifle: ");
        response.AppendLine($"- Pequeno: {CargaArmasPMin}-{CargaArmasPMax} caixas. Peso: {CargaArmasPMin * PesoRifle}-{CargaArmasPMax * PesoRifle} kgs.");
        response.AppendLine($"- Médio: {CargaArmasMMin}-{CargaArmasMMax} caixas. Peso: {CargaArmasMMin * PesoRifle}-{CargaArmasMMax * PesoRifle} kgs.");
        response.AppendLine($"- Grande: {CargaArmasGMin}-{CargaArmasGMax} caixas. Peso: {CargaArmasGMin * PesoRifle}-{CargaArmasGMax * PesoRifle} kgs.");

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
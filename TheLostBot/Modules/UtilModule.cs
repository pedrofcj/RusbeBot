using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using TheLostBot.Attributes;

namespace TheLostBot.Modules;

[CommandValidation(false, false)]
[RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]

public class UtilModule : ModuleBase<SocketCommandContext>
{
    #region IPVA

    private static readonly DateTime ReferenceDate = new DateTime(2022, 03, 07);

    [Command("ipva")]
    public async Task IpvaAsync()
    {
        var response = new StringBuilder();
        var proximoIpva = ReferenceDate;
        var hoje = DateTime.ParseExact(DateTime.UtcNow.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        while (proximoIpva <= hoje)
        {
            proximoIpva = proximoIpva.AddDays(12);
            if (proximoIpva == hoje)
            {
                response.AppendLine("Hoje é dia de pagar IPVA!");
            }
        }

        response.AppendLine($"Próximo IPVA será dia {proximoIpva.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}");
        await ReplyAsync(response.ToString());
    }

    #endregion
}
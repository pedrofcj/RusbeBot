using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RusbeBot.Attributes;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContextType = Discord.Commands.ContextType;

namespace RusbeBot.Modules;

[CommandValidation(false, false)]
[Discord.Commands.RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]

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

    #region Info

    [Command("info")]
    public async Task Info(SocketUser user)
    {
        var embedBuilder = new EmbedBuilder
        {
            Title = $"Informações de {user.Username}#{user.Discriminator}"
        };

        embedBuilder.AddField("Criado em", user.CreatedAt);
        embedBuilder.AddField("Entrou em", Context.Guild.Users.FirstOrDefault(guildUser => guildUser.Id == user.Id)?.JoinedAt);
        embedBuilder.AddField("Status", user.Status.ToString());
        embedBuilder.AddField("ID", user.Id);
        embedBuilder.AddField("ID do Avatar", user.AvatarId);
        embedBuilder.AddField("Usuário", user.Username + "#" + user.Discriminator);
        embedBuilder.AddField("É Bot?", user.IsBot);

        embedBuilder.ThumbnailUrl = user.GetAvatarUrl();

        embedBuilder.WithColor(Color.Red);
        embedBuilder.WithCurrentTimestamp();

        await Context.Message.ReplyAsync("", false, embedBuilder.Build());
    }

    #endregion

}
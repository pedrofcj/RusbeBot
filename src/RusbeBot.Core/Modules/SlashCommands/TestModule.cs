using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using RusbeBot.Core.Helpers;

namespace RusbeBot.Core.Modules.SlashCommands;

public class TestModule : BotInteraction<SocketSlashCommand>
{
    // [SlashCommand("sugestao", "Enviar uma sugestão")]
    // public async Task SugestaoAsync()
    // {
    //     await RespondWithModalAsync<SugestaoModal>("sugestao_modal");
    // }
    
    [SlashCommand("info", "Busca as informações de um user")]
    public async Task TesteAsync(SocketGuildUser user)
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

        await RespondAsync(embed: embedBuilder.Build(), ephemeral: true);
    }
}

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace RusbeBot.Core.Components;

public static class SugestaoEmbed
{
    public static Embed Create(this SugestaoModal modal, SocketInteractionContext<SocketModal> context)
    {
        var embedBuilder = new EmbedBuilder
        {
            Title = $"Sugestão enviada por {MentionUtils.MentionUser(context.User.Id)}"
        };
        
        embedBuilder.AddField("Sugestão", modal.Description);
        
        embedBuilder.ThumbnailUrl = context.User.GetAvatarUrl();

        embedBuilder.WithColor(Color.Gold);
        embedBuilder.WithCurrentTimestamp();

        return embedBuilder.Build();
    }
}
using Discord;
using IMDbApiLib;
using MediatR;
using RusbeBot.Core.Statics;

namespace RusbeBot.Core.Events;

public class GetActorsHandler : INotificationHandler<GetActorsEvent>
{
    private readonly ApiLib _apiLib;

    public GetActorsHandler(ApiLib apiLib)
    {
        _apiLib = apiLib;
    }

    public async Task Handle(GetActorsEvent notification, CancellationToken cancellationToken)
    {
        var titleId = notification.Component.Data.CustomId.Replace("getActors ", "");

        if (string.IsNullOrEmpty(titleId))
        {
            await notification.Component.RespondAsync("Ocorreu um erro ao tentar obter os atores. Tente novamente mais tarde.");
            return;
        }
        {
            
        }
        var titleData = MoviesStatics.Movies.FirstOrDefault(x => x.Key == titleId).Value;
        
        if (titleData == null)
        {
            await notification.Component.RespondAsync("Ocorreu um erro ao tentar obter os atores. Tente novamente mais tarde.");
            return;
        }
        
        var embed = new EmbedBuilder();
        embed.WithTitle($"Atores em {titleData.FullTitle}");
        embed.WithColor(new Color(0xFF0000));

        foreach (var actor in titleData.ActorList)
        {
            embed.AddField(actor.Name, actor.AsCharacter);
            if (embed.Fields.Count != 25) continue;
            
            await notification.Component.Channel.SendMessageAsync(embed: embed.Build());
            embed = new EmbedBuilder();
            embed.WithTitle($"Atores em {titleData.FullTitle}");
            embed.WithColor(new Color(0xFF0000));
        }

        await notification.Component.RespondAsync(embed: embed.Build());
    }
}
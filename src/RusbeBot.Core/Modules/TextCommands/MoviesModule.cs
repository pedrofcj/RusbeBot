using Discord;
using Discord.Commands;
using IMDbApiLib;
using RusbeBot.Core.Statics;

namespace RusbeBot.Core.Modules.TextCommands;

public class MoviesModule : ModuleBase<SocketCommandContext>
{
    private readonly ApiLib _apiLib;

    public MoviesModule(ApiLib apiLib)
    {
        _apiLib = apiLib;
    }

    [Command("movies search")]
    public async Task MoviesSearchAsync([Remainder] string search)
    {
        var movies = await _apiLib.SearchMovieAsync(search);

        foreach (var result in movies.Results)
        {
            var embedBuilder = new EmbedBuilder
            {
                Title = $"{result.Title} {result.Description}"
            };

            embedBuilder.AddField("IMDB: ", $"<https://www.imdb.com/title/{result.Id}>");
            embedBuilder.AddField("Id: ", result.Id);

            embedBuilder.ThumbnailUrl = result.Image;

            embedBuilder.WithColor(Color.Red);
            embedBuilder.WithCurrentTimestamp();

            await Context.Message.ReplyAsync("", false, embedBuilder.Build());
        }
    }

    [Command("movies id")]
    public async Task MoviesIdAsync([Remainder] string search)
    {
        var title = await _apiLib.TitleAsync(search, FullActor: true, FullCast: true);

        var embedBuilder = new EmbedBuilder()
            .WithTitle(title.FullTitle)
            .WithThumbnailUrl(title.Image)
            .WithColor(Color.Red)
            .WithCurrentTimestamp();
        
        embedBuilder.AddField("IMDB: ", $"<https://www.imdb.com/title/{title.Id}>");
        embedBuilder.AddField("Prêmios", title.Awards);
        embedBuilder.AddField("Diretores", title.Directors);
        embedBuilder.AddField("Roteiristas", title.Writers);
        embedBuilder.AddField("Genres", title.Genres);
        embedBuilder.AddField("Nota IMDB", title.IMDbRating);
        embedBuilder.AddField("Votos IMDB", Convert.ToInt32(title.IMDbRatingVotes).ToString("N0"));
        embedBuilder.AddField("Nota Metacritics", title.MetacriticRating);

        var components = new ComponentBuilder()
            .WithButton("Ver atores", "getActors " + title.Id);
        
        MoviesStatics.Movies.Add(title.Id, title);
        
        await Context.Message.ReplyAsync("", false, embedBuilder.Build(), components: components.Build());
    }
}
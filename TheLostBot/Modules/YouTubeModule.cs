using System.IO;
using System.Threading.Tasks;
using CliWrap;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace RusbeBot.Modules;

public class YouTubeModule : ModuleBase<SocketCommandContext>
{

    private readonly DiscordSocketClient _discord;
    public YouTubeModule(DiscordSocketClient discord)
    {
        _discord = discord;
    }

    [Command("ytp", RunMode = RunMode.Async)]

    public async Task YouTubePlayAsync([Remainder] string input)
    {
        var voiceChannel = (Context.User as IVoiceState)?.VoiceChannel;
        if (voiceChannel == null)
            return;

        var audioClient = await voiceChannel.ConnectAsync();

        var youtube = new YoutubeClient();
        var videoId = VideoId.Parse(input ?? "");
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
        var streamInfo = streamManifest.GetAudioOnlyStreams().TryGetWithHighestBitrate();

        if (streamInfo is null)
        {
            await Context.Message.ReplyAsync("Não foi possível tocar o áudio do video solicitado.");
            return;
        }

        var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

        var memoryStream = new MemoryStream();
        await Cli.Wrap("ffmpeg")
            .WithArguments(" -hide_banner -loglevel panic -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1")
            .WithStandardInputPipe(PipeSource.FromStream(stream))
            .WithStandardOutputPipe(PipeTarget.ToStream(memoryStream))
            .ExecuteAsync();

        await using var discord = audioClient.CreatePCMStream(AudioApplication.Mixed);
        
        try
        {
            await discord.WriteAsync(memoryStream.ToArray(), 0, (int) memoryStream.Length);
        }
        finally
        {
            await discord.FlushAsync();
            await voiceChannel.DisconnectAsync();
        }
    }

}
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using RusbeBot.Core.Components;
using RusbeBot.Core.Helpers;

namespace RusbeBot.Core.Handlers;

public class ModalHandlers : BotInteraction<SocketModal>
{
    [ModalInteraction("sugestao_modal")]
    public async Task HandleSugestaoModal(SugestaoModal modal)
    {
        const ulong channelId = 979881666036265000;
        var channel = Context.Guild.GetTextChannel(channelId);
        var embed = new EmbedBuilder()
            .WithTitle("Nova sugestão")
            .WithDescription($"{Context.User.Mention} enviou uma nova sugestão")
            .AddField("Sugestão", modal.Description)
            .WithColor(Color.Blue)
            .WithCurrentTimestamp();
        
        var message = await channel.SendMessageAsync(embed: embed.Build());
        await message.AddReactionAsync(new Emoji("✅"));
        await message.AddReactionAsync(new Emoji("❌"));

        await RespondAsync($"Sua sugestão foi enviada com sucesso!", ephemeral: true);
    }
}
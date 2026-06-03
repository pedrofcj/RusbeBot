using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using RusbeBot.Core.Extensions;
using RusbeBot.Core.Helpers;

namespace RusbeBot.Core.Modules.SlashCommands;

public class AdminModule: BotInteraction<SocketSlashCommand>
{
    [SlashCommand("purge", "Apaga todas as mensagens do usuário mencionado no período de tempo especificado")]
    public async Task PurgeAsync(SocketGuildUser user, string period)
    {
        // go thru all the channels in the guild
        foreach (var channel in Context.Guild.Channels)
        {
            // check if the channel is a text channel
            if (channel is not SocketTextChannel textChannel)
                continue;

            // get the messages from the channel
            var messages = await textChannel.GetMessagesAsync().FlattenAsync();
            
            // get time span from text
            var timeSpan = period.ToTimeSpan();

            // go thru all the messages
            foreach (var message in messages)
            {
                // check if the message author is the user we want to purge
                if (message.Author.Id != user.Id) continue;
                
                // check if the message is older than the period
                if (DateTimeOffset.UtcNow - message.Timestamp < timeSpan)
                {
                    // delete the message
                    await message.DeleteAsync();
                }
            }
        }
    }
   
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using TheLostBot.Values.TheLost;

namespace TheLostBot.Attributes
{
    public class AllowedChannels : PreconditionAttribute
    {

        private List<TheLostChannelsEnum> AllowedChannelsList { get; }

        public AllowedChannels(params TheLostChannelsEnum[] channels)
        {
            AllowedChannelsList = new List<TheLostChannelsEnum>(channels);
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var channelId = context.Channel.Id;

            return Task.FromResult(AllowedChannelsList.Any(d => (ulong)d == channelId) ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(ErrorMessage ?? "Este comando não pode ser utilizado nesta sala."));
        }

    }
}
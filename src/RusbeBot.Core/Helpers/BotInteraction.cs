using Discord.Interactions;
using Discord.WebSocket;

namespace RusbeBot.Core.Helpers;

public class BotInteraction<TInteraction> : InteractionModuleBase<SocketInteractionContext<TInteraction>> where TInteraction : SocketInteraction
{
    
}
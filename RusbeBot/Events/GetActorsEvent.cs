using Discord.WebSocket;
using MediatR;

namespace RusbeBot.Events;

public class GetActorsEvent : INotification
{
    public GetActorsEvent(SocketMessageComponent component)
    {
        Component = component;
    }

    public SocketMessageComponent Component { get;  }
}
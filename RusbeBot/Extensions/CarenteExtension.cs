using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RusbeBot.Extensions;

public static class CarenteExtension
{

    private static readonly List<string> _messages = new List<string>
    {
        "Olá, {0}! Quanto tempo!",
        "Tá falando muito, {0}!",
        "Tem como fazer silêncio aí, {0}? To tentando dormir.",
        "Saudades, {0}.",
        "E trabalhar, {0}? Quando vai rolar?"
    };

    public static async Task Interagir(this SocketUserMessage message, Random random)
    {
        var chance = random.Next(1, 50);
        if (chance == 1)
        {
            var mensagem = _messages[random.Next(0, _messages.Count - 1)];
            await message.Channel.SendMessageAsync(string.Format(mensagem, message.Author.Mention));
        }
    }

}
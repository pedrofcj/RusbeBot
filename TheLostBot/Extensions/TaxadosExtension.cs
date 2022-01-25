using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace TheLostBot.Extensions
{
    public static class TaxadosExtension
    {
        public static List<ulong> IdsTaxados = new List<ulong>();

        public static async Task VerificarTaxado(this SocketUserMessage message)
        {
            if (IdsTaxados.Contains(message.Author.Id))
            {
                await message.DeleteAsync();
            }
        }
    }
}
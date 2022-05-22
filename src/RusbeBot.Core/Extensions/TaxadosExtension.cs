using Discord.Commands;

namespace RusbeBot.Core.Extensions;

public static class TaxadosExtension
{
    private static readonly Dictionary<ulong, List<ulong>> IdsTaxados = new();

    public static void Add(ulong guildId, ulong userId)
    {
        // verificar se o server ja está no dictionary
        if (!IdsTaxados.ContainsKey(guildId))
        {
            // se não tiver, adiciona
            IdsTaxados.Add(guildId, new List<ulong> { userId });
            return;
        }

        // verifica se o user já está taxado nesse server
        var taxados = IdsTaxados[guildId];
        if (taxados.Contains(userId)) return;

        // se não tiver, adiciona
        taxados.Add(userId);
    }

    public static void Remove(ulong guildId, ulong userId)
    {
        if (!IdsTaxados.ContainsKey(guildId)) return;

        var taxados = IdsTaxados.Where(pair => pair.Key == guildId).Select(pair => pair.Value).SingleOrDefault() ?? new List<ulong>();
        taxados.Add(userId);
    }

    public static async Task VerificarTaxado(this SocketCommandContext message)
    {
        if (message.IsPrivate) return;

        if (IdsTaxados.ContainsKey(message.Guild.Id) && IdsTaxados[message.Guild.Id].Contains(message.User.Id))
            await message.Message.DeleteAsync();
    }
}
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheLostBot.Attributes;
using TheLostBot.Values.TheLost;

namespace TheLostBot.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [RequiredRoles(TheLostRolesEnum.Conselho)]
        public async Task Say([Remainder] string text)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(text);
        }

        [Command("msg")]
        [RequiredRoles(TheLostRolesEnum.Conselho)]
        public async Task Message(SocketGuildUser user, [Remainder] string text)
        {
            await Context.Message.DeleteAsync();
            await user.SendMessageAsync(text);
        }
    }
}
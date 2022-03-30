using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Discord.Commands;
using Discord.WebSocket;
using TheLostBot.Attributes;

namespace TheLostBot.Modules
{
    [RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]
    [CommandValidation(false, false)]
    public class ZoeiraModule : ModuleBase<SocketCommandContext>
    {

        private readonly ITheLostPicturesService _theLostPicturesService;
        private readonly Random _random;

        public ZoeiraModule(ITheLostPicturesService theLostPicturesService, Random rand)
        {
            _theLostPicturesService = theLostPicturesService;
            _random = rand;
        }

        #region Pics

        private static List<TheLostPictures> _pics;
        private List<TheLostPictures> Pics => _pics ??= _theLostPicturesService.GetAllAsync().GetAwaiter().GetResult();

        [Command("pic")]
        public async Task PicAsync(string category)
        {
            var categoryList = Pics.Where(d => d.Category == category && d.GuildId == Context.Guild.Id.ToString()).ToList();
            var random = _random.Next(0, categoryList.Count - 1);
            await ReplyAsync(categoryList[random].Url);
        }

        [Command("addpic")]
        public async Task AddPicAsync(string category, string url)
        {
            var newPic = new TheLostPictures
            {
                Id = Guid.NewGuid().ToString(),
                Category = category,
                Url = url,
                GuildId = Context.Guild.Id.ToString()
            };

            await _theLostPicturesService.InsertAsync(newPic);
            var msg = await ReplyAsync("Nova foto adicionada!");
            await Context.Message.DeleteAsync();
            _pics = null;

            await Task.Delay(3000);

            await msg.DeleteAsync();
        }

        #endregion

        #region Buzz

        [Command("buzz")]
        public async Task Buzz(SocketGuildUser user, [Remainder] int quantidade)
        {
            await Context.Message.DeleteAsync();

            for (var i = 0; i < quantidade; i++)
            {
                await ReplyAsync(user.Mention);
                await Task.Delay(2000);
            }
        }

        #endregion

        #region Avatar

        [Command("avatar")]
        public async Task AvatarAsync(SocketGuildUser user)
        {
            var url = user.GetAvatarUrl(size: 2048);
            await ReplyAsync(url);
        }

        #endregion

    }
}
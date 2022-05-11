using Data.Interfaces;
using Data.Models;
using Discord.Commands;
using Discord.WebSocket;
using RusbeBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RusbeBot.Modules;

[RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]
[CommandValidation(false, false)]
public class ZoeiraModule : ModuleBase<SocketCommandContext>
{

    private readonly IPicsService _picsService;
    private readonly Random _random;

    public ZoeiraModule(IPicsService picsService, Random rand)
    {
        _picsService = picsService;
        _random = rand;
    }

    #region Pics

    private static List<PicsModel> _pics;
    private List<PicsModel> Pics => _pics ??= _picsService.GetAllAsync().GetAwaiter().GetResult();

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
        var newPic = new PicsModel
        {
            Id = Guid.NewGuid().ToString(),
            Category = category,
            Url = url,
            GuildId = Context.Guild.Id.ToString()
        };

        await _picsService.InsertAsync(newPic);
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
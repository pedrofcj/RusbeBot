using System.Text;
using Discord;
using Discord.Commands;
using RusbeBot.Core.Attributes;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Core.Modules.TextCommands;

[OwnerValidation]
public class OwnerModule : ModuleBase<SocketCommandContext>
{
    private readonly IModeradorService _moderadorService;
    public OwnerModule(IModeradorService moderadorService)
    {
        _moderadorService = moderadorService;
    }

    [Command("add moderador")]
    public async Task AddModeradorAsync(ulong userId)
    {
        var existing = await _moderadorService.GetModeradorByUserIdAsync(userId.ToString());

        if (existing != null)
        {
            await Context.User.SendMessageAsync($"User {MentionUtils.MentionUser(userId)} já é um moderador.");
            return;
        }

        await _moderadorService.InsertAsync(new ModeradorModel
        {
            UserId = userId.ToString()
        });

        await Context.User.SendMessageAsync($"User {MentionUtils.MentionUser(userId)} adicionado com sucesso");
    }

    [Command("remove moderador")]
    public async Task RemoveModeradorAsync(ulong userId)
    {
        var existing = await _moderadorService.GetModeradorByUserIdAsync(userId.ToString());

        if (existing is null)
        {
            await Context.User.SendMessageAsync($"User {MentionUtils.MentionUser(userId)} não é um moderador.");
            return;
        }

        await _moderadorService.DeleteAsync(existing.Id);
        await Context.User.SendMessageAsync($"User {MentionUtils.MentionUser(userId)} removido com sucesso");
    }

    [Command("list moderador")]
    public async Task ListModeradorAsync()
    {
        var response = new StringBuilder();
        response.AppendLine("Moderadores:");

        var moderators = await _moderadorService.GetAllAsync();
        response.AppendLine(string.Join(Environment.NewLine, moderators.Select(model => MentionUtils.MentionUser(Convert.ToUInt64(model.UserId)))));
        await Context.User.SendMessageAsync(response.ToString());
    }

}
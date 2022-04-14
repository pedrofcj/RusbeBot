using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Discord;
using Discord.Commands;
using RusbeBot.Attributes;

namespace RusbeBot.Modules;

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
        var existente = await _moderadorService.GetModeradorByUserIdAsync(userId.ToString());

        if (existente != null)
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
        var existente = await _moderadorService.GetModeradorByUserIdAsync(userId.ToString());

        if (existente is null)
        {
            await Context.User.SendMessageAsync($"User {MentionUtils.MentionUser(userId)} não é um moderador.");
            return;
        }

        await _moderadorService.DeleteAsync(existente.Id);
        await Context.User.SendMessageAsync($"User {MentionUtils.MentionUser(userId)} removido com sucesso");
    }

    [Command("list moderador")]
    public async Task ListModeradorAsync()
    {
        var response = new StringBuilder();
        response.AppendLine("Moderadores:");

        var moderadores = await _moderadorService.GetAllAsync();
        response.AppendLine(string.Join(Environment.NewLine, moderadores.Select(model => MentionUtils.MentionUser(Convert.ToUInt64(model.UserId)))));
        await Context.User.SendMessageAsync(response.ToString());
    }

}
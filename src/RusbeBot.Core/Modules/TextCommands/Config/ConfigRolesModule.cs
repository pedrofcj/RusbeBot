using System.Text;
using Discord;
using Discord.Commands;
using RusbeBot.Core.Attributes;
using RusbeBot.Data.Interfaces;
using RusbeBot.Data.Models;

namespace RusbeBot.Core.Modules.TextCommands.Config;

[CommandValidation(false, true)]
[RequireContext(ContextType.Guild, ErrorMessage = "Este comando só pode ser utilizado em um servidor")]
public class ConfigRolesModule : ConfigModule
{
    public ConfigRolesModule(IAllowedChannelsConfigService allowedChannelsConfigService, IAllowedRolesConfigService allowedRolesConfigService) : base(allowedChannelsConfigService, allowedRolesConfigService)
    {
    }

    [Command("roles add")]
    public async Task AddConfigRoleAsync([Remainder] string input)
    {
        try
        {
            var command = input.Split(' ')[0];

            foreach (var socketRole in Context.Message.MentionedRoles)
            {
                await AllowedRolesConfigService.InsertAsync(new AllowedRolesConfigModel
                {
                    GuildId = Context.Guild.Id.ToString(),
                    CommandName = command,
                    RoleId = socketRole.Id.ToString()
                });
            }

            await ReplyAsync("Roles adicionadas com sucesso");
        }
        catch (Exception e)
        {
            await ReplyAsync("Houve um erro ao tentar adicionar as roles");
            Console.WriteLine(e);
            throw;
        }
    }

    [Command("roles remove")]
    public async Task RemoveConfigRoleAsync([Remainder] string input)
    {
        try
        {
            var command = input.Split(' ')[0];

            var allAllowedRoles = await AllowedRolesConfigService.GetAllowedRolesByCommandAndGuild(command, Context.Guild.Id.ToString());

            foreach (var socketRole in Context.Message.MentionedRoles)
            {
                var role = allAllowedRoles.SingleOrDefault(model => model.RoleId == socketRole.Id.ToString());

                if (role != null)
                {
                    await AllowedRolesConfigService.DeleteAsync(role.Id);
                }
            }

            await ReplyAsync("Roles removidas com sucesso");
        }
        catch (Exception e)
        {
            await ReplyAsync("Houve um erro ao tentar remover as roles");
            Console.WriteLine(e);
            throw;
        }
    }

    [Command("roles list")]
    public async Task ListConfigRoleAsync([Remainder] string input)
    {
        try
        {
            var command = input.Split(' ')[0];

            var allAllowedRoles = await AllowedRolesConfigService.GetAllowedRolesByCommandAndGuild(command, Context.Guild.Id.ToString());

            var response = new StringBuilder();
            response.AppendLine("Roles que podem usar este comando: ");

            foreach (var allowedRole in allAllowedRoles)
            {
                response.AppendLine(MentionUtils.MentionRole(Convert.ToUInt64(allowedRole.RoleId)));
            }

            await ReplyAsync(response.ToString());
        }
        catch (Exception e)
        {
            await ReplyAsync("Houve um erro ao tentar buscar as roles que podem executar esse comando");
            Console.WriteLine(e);
            throw;
        }
    }

}
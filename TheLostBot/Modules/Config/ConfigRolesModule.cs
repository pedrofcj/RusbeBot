using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Discord;
using Discord.Commands;
using TheLostBot.Attributes;

namespace TheLostBot.Modules.Config;

public class ConfigRolesModule : ConfigModule
{
    public ConfigRolesModule(IAllowedChannelsConfigService allowedChannelsConfigService, IAllowedRolesConfigService allowedRolesConfigService) : base(allowedChannelsConfigService, allowedRolesConfigService)
    {
    }

    [Command("roles add")]
    [RequiredRoles(true)]
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
    [RequiredRoles(true)]
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
    [RequiredRoles(true)]
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
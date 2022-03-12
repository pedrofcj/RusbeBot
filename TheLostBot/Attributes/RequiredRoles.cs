using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheLostBot.Values.TheLost;

namespace TheLostBot.Attributes
{
    public class RequiredRoles : PreconditionAttribute
    {
        private List<ulong> AuthorizedRoles { get; }

        public RequiredRoles(params ulong[] roles)
        {
            AuthorizedRoles = new List<ulong>(roles);
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (!(context.User is IGuildUser user))
                return Task.FromResult(PreconditionResult.FromError("Command must be used in a guild channel."));

            var userRoles = user.RoleIds.ToList();
            
            return Task.FromResult(AuthorizedRoles.Intersect(userRoles).Any() ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(ErrorMessage ?? "Você não tem permissão para executar esse comando."));
        }
    }
}
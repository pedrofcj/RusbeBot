using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace RusbeBot.Attributes;

public class OwnerValidation : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        var application = await context.Client.GetApplicationInfoAsync();
        return context.User.Id == application.Owner.Id 
            ? PreconditionResult.FromSuccess() 
            : PreconditionResult.FromError("Você não é o dono do bot e não pode executar esse comando");
    }
}
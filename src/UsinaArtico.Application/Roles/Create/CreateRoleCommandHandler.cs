using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Roles.Create;

internal sealed class CreateRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager)
    : ICommandHandler<CreateRoleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        if (await roleManager.RoleExistsAsync(command.Name))
        {
            return Result.Failure<Guid>(Error.Conflict("Identity.RoleExists", $"Role {command.Name} already exists"));
        }

        var role = new IdentityRole<Guid>(command.Name);
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return Result.Failure<Guid>(Error.Failure("Identity.RoleCreation", result.Errors.First().Description));
        }

        return role.Id;
    }
}

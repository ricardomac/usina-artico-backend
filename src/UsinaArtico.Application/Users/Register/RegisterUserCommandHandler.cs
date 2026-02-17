using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Enums;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel.Authorization;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = command.Email,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        IdentityResult result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault();
            return Result.Failure<Guid>(Error.Conflict("Identity.Error", firstError?.Description ?? "Unknown error"));
        }

        
        var roleExists = await roleManager.RoleExistsAsync(command.RoleName);
        if (!roleExists)
        {
             return Result.Failure<Guid>(Error.NotFound("Identity.Role", $"Role {command.RoleName} not found"));
        }

        await userManager.AddToRoleAsync(user, command.RoleName);


        return user.Id;
    }
}

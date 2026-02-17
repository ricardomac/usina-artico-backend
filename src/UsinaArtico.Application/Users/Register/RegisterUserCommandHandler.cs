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

        // Assign Role
        string roleName = command.NivelAcesso.ToString(); // "Usuario", "Vendedor", "Admin"
        
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
             return Result.Failure<Guid>(Error.NotFound("Identity.Role", $"Role {roleName} not found"));
        }

        await userManager.AddToRoleAsync(user, roleName);

        // Add NivelAcesso claim
        await userManager.AddClaimAsync(user, new Claim("nivel_acesso", ((int)command.NivelAcesso).ToString()));

        return user.Id;
    }
}

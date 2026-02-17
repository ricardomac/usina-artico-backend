using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.Update;

internal sealed class UpdateUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == command.Id, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.Id));
        }

        // Update basic info
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;

        // If email changes, update username/email
        if (!string.Equals(user.Email, command.Email, StringComparison.OrdinalIgnoreCase))
        {
            user.Email = command.Email;
            user.UserName = command.Email;
        }

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var firstError = updateResult.Errors.FirstOrDefault();
            return Result.Failure(Error.Conflict("Identity.Error", firstError?.Description ?? "Unknown error"));
        }

        // Change password if provided (optional)
        if (!string.IsNullOrWhiteSpace(command.Password))
        {
            // generate reset token and reset password to avoid needing current password
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var passResult = await userManager.ResetPasswordAsync(user, resetToken, command.Password);
            if (!passResult.Succeeded)
            {
                var firstError = passResult.Errors.FirstOrDefault();
                return Result.Failure(Error.Conflict("Identity.Password", firstError?.Description ?? "Unknown error"));
            }
        }

        // Update role
        var roleExists = await roleManager.RoleExistsAsync(command.RoleName);
        if (!roleExists)
        {
            return Result.Failure(Error.NotFound("Identity.Role", $"Role {command.RoleName} not found"));
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            await userManager.RemoveFromRolesAsync(user, currentRoles);
        }
        await userManager.AddToRoleAsync(user, command.RoleName);

        return Result.Success();
    }
}
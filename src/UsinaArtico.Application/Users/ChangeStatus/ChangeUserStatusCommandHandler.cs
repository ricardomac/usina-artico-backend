using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.ChangeStatus;

internal sealed class ChangeUserStatusCommandHandler(UserManager<User> userManager)
    : ICommandHandler<ChangeUserStatusCommand>
{
    public async Task<Result> Handle(ChangeUserStatusCommand command, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        user.IsActive = command.IsActive;

        IdentityResult result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault();
            return Result.Failure(Error.Failure("Identity.Error", firstError?.Description ?? "Unknown error"));
        }

        return Result.Success();
    }
}

using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.Login;

internal sealed class LoginUserCommandHandler(UserManager<User> userManager)
    : ICommandHandler<LoginUserCommand, User>
{
    public async Task<Result<User>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            return Result.Failure<User>(UserErrors.NotFoundByEmail);
        }

        if (!user.IsActive)
        {
            return Result.Failure<User>(UserErrors.Inactive);
        }

        bool verified = await userManager.CheckPasswordAsync(user, command.Password);

        if (!verified)
        {
            return Result.Failure<User>(UserErrors.NotFoundByEmail); // Ambiguous error for security
        }

        return user;
    }
}

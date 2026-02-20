using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Abstractions.Services;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Identity.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    UserManager<User> userManager,
    IEmailService emailService)
    : ICommandHandler<ForgotPasswordCommand>
{
    public async Task<Result> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is not null)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            await emailService.SendForgotPasswordEmailAsync(user.Email!, code, cancellationToken);
        }

        // Don't reveal that the user does not exist
        return Result.Success();
    }
}

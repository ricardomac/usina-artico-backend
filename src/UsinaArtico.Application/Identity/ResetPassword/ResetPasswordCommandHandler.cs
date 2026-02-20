using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Identity.ResetPassword;

internal sealed class ResetPasswordCommandHandler(UserManager<User> userManager)
    : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            // Don't reveal that the user does not exist
            return Result.Failure(Error.Failure("Identity.InvalidRequest", "Solicitação inválida."));
        }

        var identityResult = await userManager.ResetPasswordAsync(user, command.ResetCode, command.NewPassword);
        if (identityResult.Succeeded)
        {
            return Result.Success();
        }

        var firstError = identityResult.Errors.FirstOrDefault();
        return Result.Failure(Error.Failure("Identity.ResetPasswordFailed",
            firstError?.Description ?? "Erro ao resetar senha."));
    }
}

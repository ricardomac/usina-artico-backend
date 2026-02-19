using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.AlterarSenha;

internal sealed class AlterarSenhaCommandHandler(
    UserManager<User> userManager,
    IUserContext userContext)
    : ICommandHandler<AlterarSenhaCommand>
{
    public async Task<Result> Handle(AlterarSenhaCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userContext.UserId.ToString());

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(userContext.UserId));
        }

        var result = await userManager.ChangePasswordAsync(
            user,
            command.SenhaAtual,
            command.NovaSenha);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault();
            return Result.Failure(Error.Problem(
                "Users.ChangePasswordFailed",
                firstError?.Description ?? "Ocorreu um erro ao trocar a senha."));
        }

        return Result.Success();
    }
}

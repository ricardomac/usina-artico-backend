using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.Excluir;

internal sealed class ExcluirUsuarioCommandHandler(
    UserManager<User> userManager,
    IUserContext userContext)
    : ICommandHandler<ExcluirUsuarioCommand>
{
    public async Task<Result> Handle(ExcluirUsuarioCommand command, CancellationToken cancellationToken)
    {
        if (command.UserId == userContext.UserId)
        {
            return Result.Failure(Error.Problem(
                "Users.DeleteSelf",
                "Você não pode excluir seu próprio usuário."));
        }

        var user = await userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault();
            return Result.Failure(Error.Problem(
                "Users.DeleteFailed",
                firstError?.Description ?? "Ocorreu um erro ao excluir o usuário."));
        }

        return Result.Success();
    }
}

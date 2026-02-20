using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Identity.Login;

internal sealed class LoginCommandHandler(
    UserManager<User> userManager,
    ITokenProvider tokenProvider)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Identity.InvalidCredential", "Credenciais Inválidas"));
        }

        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Identity.Inactive", "O usuário está inativo."));
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return Result.Failure<LoginResponse>(Error.Problem("Identity.IsLockedOut", "A conta está bloqueada."));
        }

        var verified = await userManager.CheckPasswordAsync(user, command.Password);

        if (!verified)
        {
            await userManager.AccessFailedAsync(user);
            return Result.Failure<LoginResponse>(Error.Problem("Identity.InvalidCredential", "Credenciais Inválidas"));
        }

        await userManager.ResetAccessFailedCountAsync(user);

        if (command.UseCookies || command.UseSessionCookies)
        {
            // O login via cookie deve ser tratado na camada da API, pois depende do HttpContext/SignInManager.
            return new LoginResponse(null, true, user);
        }

        var token = tokenProvider.Create(user);

        return new LoginResponse(token);
    }
}

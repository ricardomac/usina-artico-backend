using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Services;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class Auth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags(Tags.Auth);

        group.MapPost("login", async (
            [FromBody] LoginRequest request,
            [FromQuery] bool? useCookies,
            [FromQuery] bool? useSessionCookies,
            SignInManager<User> signInManager,
            ITokenProvider tokenProvider) =>
        {
            var useCookieScheme = useCookies == true || useSessionCookies == true;
            var isPersistent = useCookies == true && useSessionCookies != true;

            signInManager.AuthenticationScheme = useCookieScheme
                ? IdentityConstants.ApplicationScheme
                : IdentityConstants.BearerScheme;

            var signInResult = await signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent,
                lockoutOnFailure: true);

            if (signInResult.Succeeded)
            {
                if (useCookieScheme)
                {
                    return Result.Success().Match(() => Results.Ok(), CustomResults.Problem);
                }

                var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
                var token = tokenProvider.Create(user!);

                return Result.Success(new { AccessToken = token }).Match(Results.Ok, CustomResults.Problem);
            }

            if (signInResult.RequiresTwoFactor)
            {
                return Result.Failure(Error.Problem("Identity.RequiresTwoFactor", "Requer autenticação de dois fatores."))
                    .Match(() => Results.Ok(), CustomResults.Problem);
            }

            if (signInResult.IsLockedOut)
            {
                return Result.Failure(Error.Problem("Identity.IsLockedOut", "A conta está bloqueada."))
                    .Match(() => Results.Ok(), CustomResults.Problem);
            }

            return Result.Failure(Error.Problem("Identity.InvalidCredential", "Credenciais Inválidas")).Match(() => Results.Ok(), CustomResults.Problem);
        });

        group.MapPost("forgot-password", async (
            [FromBody] ForgotPasswordRequest request,
            UserManager<User> userManager,
            IEmailService emailService) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                await emailService.SendForgotPasswordEmailAsync(user.Email!, code);
            }

            // Don't reveal that the user does not exist
            return Result.Success().Match(() => Results.Ok(), CustomResults.Problem);
        });

        group.MapPost("reset-password", async (
            [FromBody] ResetPasswordRequest request,
            UserManager<User> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                // Don't reveal that the user does not exist
                return Result.Failure(Error.Failure("Identity.InvalidRequest", "Solicitação inválida."))
                    .Match(() => Results.Ok(), CustomResults.Problem);
            }

            var identityResult = await userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
            if (identityResult.Succeeded)
            {
                return Result.Success().Match(() => Results.Ok(), CustomResults.Problem);
            }

            var firstError = identityResult.Errors.FirstOrDefault();
            return Result.Failure(Error.Failure("Identity.ResetPasswordFailed",
                firstError?.Description ?? "Erro ao resetar senha.")).Match(() => Results.Ok(), CustomResults.Problem);
        });
    }

    public sealed record LoginRequest(string Email, string Password);

    public sealed record ForgotPasswordRequest(string Email);

    public sealed record ResetPasswordRequest(string Email, string ResetCode, string NewPassword);
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Identity.Login;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class Login : IEndpoint
{
    public sealed class Request
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async (
                [FromBody] Request request,
                [FromQuery] bool? useCookies,
                [FromQuery] bool? useSessionCookies,
                ICommandHandler<LoginCommand, LoginResponse> handler,
                SignInManager<User> signInManager,
                CancellationToken cancellationToken) =>
            {
                var command = new LoginCommand(
                    request.Email,
                    request.Password,
                    useCookies == true,
                    useSessionCookies == true);

                var result = await handler.Handle(command, cancellationToken);

                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }

                var response = result.Value;

                if (!response.UseCookieScheme) return Results.Ok(new { AccessToken = response.AccessToken });

                var isPersistent = useCookies == true && useSessionCookies != true;
                signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

                // Já verificamos as credenciais no manipulador, mas SignInManager
                // é a maneira mais fácil de definir os cookies de identidade corretamente.
                await signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent,
                    lockoutOnFailure: true);

                return Results.Ok();
            })
            .WithTags(Tags.Auth)
            .WithSummary("Realiza o login do usuário");
    }
}
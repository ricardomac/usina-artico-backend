using Microsoft.AspNetCore.Mvc;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Identity.ForgotPassword;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class ForgotPassword : IEndpoint
{
    public sealed class Request
    {
        public string Email { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/forgot-password", async (
            [FromBody] Request request,
            ICommandHandler<ForgotPasswordCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ForgotPasswordCommand(request.Email);
            var result = await handler.Handle(command, cancellationToken);
            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.Auth)
        .WithSummary("Solicita redefinição de senha");
    }
}

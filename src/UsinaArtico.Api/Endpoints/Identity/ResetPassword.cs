using Microsoft.AspNetCore.Mvc;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Identity.ResetPassword;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class ResetPassword : IEndpoint
{
    public sealed class Request
    {
        public string Email { get; set; } = string.Empty;
        public string ResetCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/reset-password", async (
            [FromBody] Request request,
            ICommandHandler<ResetPasswordCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ResetPasswordCommand(request.Email, request.ResetCode, request.NewPassword);
            var result = await handler.Handle(command, cancellationToken);
            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.Auth)
        .WithSummary("Redefine a senha do usuário");
    }
}

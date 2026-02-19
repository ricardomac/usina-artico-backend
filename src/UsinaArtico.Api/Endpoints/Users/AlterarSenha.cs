using UsinaArtico.Api.Endpoints;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.AlterarSenha;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Users;

public sealed class AlterarSenha : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/alterar-senha", async (
            AlterarSenhaRequest request,
            ICommandHandler<AlterarSenhaCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new AlterarSenhaCommand(
                request.SenhaAtual, 
                request.NovaSenha, 
                request.ConfirmarSenha);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Users);
    }
}

public sealed record AlterarSenhaRequest(string SenhaAtual, string NovaSenha, string ConfirmarSenha);

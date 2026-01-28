using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.Create;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Clientes;

public sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty; // Changed name
        public string CodigoCliente { get; set; } = string.Empty;
        public int Tipo { get; set; } 
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("clientes", async (
            Request request,
            ICommandHandler<CreateClienteCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateClienteCommand(
                request.Nome,
                request.Email,
                request.Telefone,
                request.Documento,
                request.CodigoCliente,
                (TipoPessoa)request.Tipo);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Clientes)
        .RequireAuthorization()
        .WithSummary("Cria um novo cliente")
        .WithDescription("Cria um novo cliente validando CPF ou CNPJ de acordo com o tipo de pessoa informado (1: Física, 2: Jurídica).")
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}

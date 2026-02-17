using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.Create;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Clientes;

public sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string CodigoCliente { get; set; } = string.Empty;
        public List<EnderecoRequest> Enderecos { get; set; } = [];
    }

    public sealed class EnderecoRequest
    {
        public string CodigoInstalacao { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public int TipoLigacao { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public ContratoRequest? Contrato { get; set; }
    }

    public sealed class ContratoRequest
    {
        public decimal ValorKwh { get; set; }
        public decimal QuantidadeKwh { get; set; }
        public DateTime DataInicio { get; set; }
        public string Anexo { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/clientes", async (
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
                request.Enderecos.Select(e => new EnderecoDto(
                    e.CodigoInstalacao,
                    e.Logradouro,
                    e.TipoLigacao,
                    e.Cep,
                    e.Numero,
                    e.Bairro,
                    e.Cidade,
                    e.Uf,
                    e.Contrato != null ? new ContratoDto(
                        e.Contrato.ValorKwh,
                        e.Contrato.QuantidadeKwh,
                        e.Contrato.DataInicio,
                        e.Contrato.Anexo
                    ) : null
                )).ToList());

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.ClientesWrite)
        .WithTags(Tags.Clientes)
        .WithSummary("Cria um novo cliente")
        .WithDescription("Cria um novo cliente com endereços e contratos opcionais.")
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}

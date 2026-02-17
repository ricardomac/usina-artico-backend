using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.Update;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Clientes;

public sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string CodigoCliente { get; set; } = string.Empty;
        public List<EnderecoUpdateRequest> Enderecos { get; set; } = [];
        public bool IsActive { get; set; }
    }

    public sealed class EnderecoUpdateRequest
    {
        public Guid? Id { get; set; }
        public string CodigoInstalacao { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public int TipoLigacao { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public ContratoUpdateRequest? Contrato { get; set; }
    }

    public sealed class ContratoUpdateRequest
    {
        public decimal ValorKwh { get; set; }
        public decimal QuantidadeKwh { get; set; }
        public DateTime DataInicio { get; set; }
        public string Anexo { get; set; } = string.Empty;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/clientes/{id}", async (
                Guid id,
                Request request,
                ICommandHandler<UpdateClienteCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateClienteCommand(
                    id,
                    request.Nome,
                    request.Email,
                    request.Telefone,
                    request.Documento,
                    request.CodigoCliente,
                    request.IsActive,
                    request.Enderecos.Select(e => new UpdateEnderecoCommand(
                        e.Id,
                        e.CodigoInstalacao,
                        e.Logradouro,
                        e.TipoLigacao,
                        e.Cep,
                        e.Numero,
                        e.Bairro,
                        e.Cidade,
                        e.Uf,
                        e.Contrato != null
                            ? new UpdateContratoCommand(
                                null,
                                e.Contrato.ValorKwh,
                                e.Contrato.QuantidadeKwh,
                                e.Contrato.DataInicio,
                                e.Contrato.Anexo
                            )
                            : null
                    )).ToList());

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.ClientesUpdate)
            .WithTags(Tags.Clientes)
            .WithSummary("Atualiza um cliente")
            .WithDescription(
                "Atualiza os dados de um cliente existente, incluindo endereços e contratos. Endereços não listados serão removidos.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Clientes;

namespace UsinaArtico.Application.Clientes.Create;

public sealed record CreateClienteCommand(
    string Nome,
    string Email,
    string Telefone,
    string Documento,
    string CodigoCliente,
    TipoPessoa Tipo) : ICommand<Guid>;

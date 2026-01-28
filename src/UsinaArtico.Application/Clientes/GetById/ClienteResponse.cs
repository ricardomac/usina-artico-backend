namespace UsinaArtico.Application.Clientes.GetById;

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string CpfCnpj,
    string CodigoCliente);

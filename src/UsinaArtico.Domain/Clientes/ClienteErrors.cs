using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Clientes;

public static class ClienteErrors
{
    public static Error NotFound(Guid clienteId) => Error.NotFound(
        "Cliente.NotFound",
        $"The cliente with the Id = '{clienteId}' was not found");

    public static Error DuplicateDocument => Error.Conflict(
        "Cliente.DuplicateCpfCnpj",
        "A cliente with this document already exists");

    public static Error InvalidDocument => Error.Validation(
        "Cliente.InvalidDocument",
        "Documento deve ter 11 (CPF) ou 14 (CNPJ) dígitos.");

    public static Error DuplicateEmail => Error.Conflict(
        "Cliente.DuplicateEmail",
        "A cliente with this Email already exists");

    public static Error DuplicateCodigoCliente => Error.Conflict(
        "Cliente.DuplicateCodigoCliente",
        "Já existe um cliente com este código.");
}

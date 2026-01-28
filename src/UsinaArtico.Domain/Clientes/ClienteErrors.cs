using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Clientes;

public static class ClienteErrors
{
    public static Error NotFound(Guid clienteId) => Error.NotFound(
        "Cliente.NotFound",
        $"The cliente with the Id = '{clienteId}' was not found");

    public static Error DuplicateCpfCnpj => Error.Conflict(
        "Cliente.DuplicateCpfCnpj",
        "A cliente with this CPF/CNPJ already exists");

    public static Error DuplicateEmail => Error.Conflict(
        "Cliente.DuplicateEmail",
        "A cliente with this Email already exists");
}

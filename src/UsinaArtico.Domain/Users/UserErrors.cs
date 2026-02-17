using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"O usuário com o Id = '{userId}' não foi encontrado");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "Você não está autorizado a realizar esta ação.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "O usuário com o email especificado não foi encontrado");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "O email fornecido não é único");

    public static readonly Error Inactive = Error.Failure(
        "Users.Inactive",
        "O usuário está inativo");
}
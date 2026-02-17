using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.Create;

public sealed record CreateUserCommand(string Email, string FirstName, string LastName, string Password, string RoleName)
    : ICommand<Guid>;

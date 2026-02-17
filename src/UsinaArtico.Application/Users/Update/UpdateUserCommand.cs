using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.Update;

public sealed record UpdateUserCommand(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? Password,
    string RoleName) : ICommand;
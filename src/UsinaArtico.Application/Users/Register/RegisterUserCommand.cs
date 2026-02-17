using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Enums;

namespace UsinaArtico.Application.Users.Register;

public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password, string RoleName)
    : ICommand<Guid>;

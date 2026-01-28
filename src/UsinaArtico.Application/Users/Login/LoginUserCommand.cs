using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;

namespace UsinaArtico.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<User>;

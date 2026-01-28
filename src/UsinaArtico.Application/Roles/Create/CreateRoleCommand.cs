using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Roles.Create;

public sealed record CreateRoleCommand(string Name) : ICommand<Guid>;

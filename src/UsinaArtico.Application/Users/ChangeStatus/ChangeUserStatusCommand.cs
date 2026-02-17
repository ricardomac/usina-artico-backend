using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.ChangeStatus;

public sealed record ChangeUserStatusCommand(Guid UserId, bool IsActive) : ICommand;

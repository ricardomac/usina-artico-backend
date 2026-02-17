using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Clientes.ChangeStatus;

public sealed record ChangeClienteStatusCommand(Guid ClienteId, bool IsActive) : ICommand;

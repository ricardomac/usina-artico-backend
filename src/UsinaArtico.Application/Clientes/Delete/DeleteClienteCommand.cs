using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Clientes.Delete;

public sealed record DeleteClienteCommand(Guid ClienteId) : ICommand;

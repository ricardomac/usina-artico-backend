using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Clientes.GetById;

public sealed record GetClienteByIdQuery(Guid ClienteId) : IQuery<ClienteResponse>;

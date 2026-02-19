using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.Excluir;

public sealed record ExcluirUsuarioCommand(Guid UserId) : ICommand;

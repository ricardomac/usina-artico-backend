using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.AlterarSenha;

public sealed record AlterarSenhaCommand(
    string SenhaAtual,
    string NovaSenha,
    string ConfirmarSenha) : ICommand;

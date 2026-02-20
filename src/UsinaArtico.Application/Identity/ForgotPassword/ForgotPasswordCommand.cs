using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Identity.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand;

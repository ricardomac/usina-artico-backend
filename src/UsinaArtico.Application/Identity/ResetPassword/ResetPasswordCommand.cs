using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Identity.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string ResetCode, string NewPassword) : ICommand;

using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendForgotPasswordEmailAsync(string email, string token, CancellationToken cancellationToken = default);
}

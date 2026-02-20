using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using UsinaArtico.Application.Abstractions.Services;

namespace UsinaArtico.Infrastructure.Services;

public sealed class EmailService(ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
{
    public async Task SendForgotPasswordEmailAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        var frontendUrl = configuration["FrontendUrl"];
        var resetLink = $"{frontendUrl}?email={WebUtility.UrlEncode(email)}&token={WebUtility.UrlEncode(token)}";

        var smtpHost = configuration["Smtp:Host"];
        var smtpPort = int.Parse(configuration["Smtp:Port"] ?? "587");
        var smtpEmail = configuration["Smtp:Email"];
        var smtpPassword = configuration["Smtp:Password"];

        var message = new MailMessage
        {
            From = new MailAddress(smtpEmail!, "Usina Ártico"),
            Subject = "Recuperação de Senha - Usina Ártico",
            Body = $@"
                <h1>Recuperação de Senha</h1>
                <p>Você solicitou a recuperação de senha para sua conta na Usina Ártico.</p>
                <p>Clique no link abaixo para definir uma nova senha:</p>
                <a href='{resetLink}'>Redefinir Senha</a>
                <p>Se você não solicitou isso, ignore este e-mail.</p>",
            IsBodyHtml = true
        };
        message.To.Add(email);

        try
        {
            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                EnableSsl = true
            };

            await client.SendMailAsync(message, cancellationToken);
            logger.LogInformation("E-mail de recuperação de senha enviado para {Email}", email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao enviar e-mail de recuperação de senha para {Email}", email);
            throw;
        }
    }
}

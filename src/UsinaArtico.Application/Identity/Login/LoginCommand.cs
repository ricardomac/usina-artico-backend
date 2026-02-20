using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;

namespace UsinaArtico.Application.Identity.Login;

public sealed record LoginCommand(
    string Email, 
    string Password, 
    bool UseCookies = false, 
    bool UseSessionCookies = false) : ICommand<LoginResponse>;

public sealed record LoginResponse(
    string? AccessToken = null, 
    bool UseCookieScheme = false, 
    User? User = null);

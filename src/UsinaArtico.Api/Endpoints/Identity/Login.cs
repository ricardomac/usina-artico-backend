using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.Login;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class Login
{
    public sealed record Request(string Email, string Password);

}

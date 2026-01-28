using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;

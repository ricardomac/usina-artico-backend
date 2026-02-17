using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.GetById;

namespace UsinaArtico.Application.Users.GetMe;

public sealed record GetMeQuery() : IQuery<UserResponse>;
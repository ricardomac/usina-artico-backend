using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;

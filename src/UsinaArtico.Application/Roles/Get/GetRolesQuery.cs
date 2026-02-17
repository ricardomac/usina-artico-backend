using System.Collections.Generic;
using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Roles.Get;

public sealed record GetRolesQuery : IQuery<List<RoleResponse>>;

public sealed record RoleResponse(Guid Id, string Name, List<string> Permissions);

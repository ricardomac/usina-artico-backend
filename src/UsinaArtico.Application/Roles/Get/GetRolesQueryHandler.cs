using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Roles.Get;

internal sealed class GetRolesQueryHandler(RoleManager<IdentityRole<Guid>> roleManager)
    : IQueryHandler<GetRolesQuery, List<RoleResponse>>
{
    public async Task<Result<List<RoleResponse>>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles
            .Select(r => new RoleResponse(r.Id, r.Name!))
            .ToListAsync(cancellationToken);

        return roles;
    }
}

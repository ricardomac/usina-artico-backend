using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.SharedKernel.Authorization;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Roles.Get;

internal sealed class GetRolesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetRolesQuery, List<RoleResponse>>
{
    public async Task<Result<List<RoleResponse>>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
    {
        var rolesWithPermissions = await context.Roles
            .AsNoTracking()
            .Select(role => new
            {
                role.Id,
                role.Name,
                Permissions = context.RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == CustomClaimTypes.Permission)
                    .Select(rc => rc.ClaimValue!)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        var response = rolesWithPermissions
            .Select(r => new RoleResponse(r.Id, r.Name!, r.Permissions))
            .ToList();

        return response;
    }
}

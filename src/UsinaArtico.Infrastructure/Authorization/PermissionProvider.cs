using Microsoft.AspNetCore.Identity;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Infrastructure.Authorization;

public sealed class PermissionProvider(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager) : IPermissionProvider
{
    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return [];
        }

        var userClaims = await userManager.GetClaimsAsync(user);
        var permissions = userClaims
            .Where(c => c.Type == CustomClaimTypes.Permission)
            .Select(c => c.Value)
            .ToHashSet();

        var roles = await userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                foreach (var claim in roleClaims.Where(c => c.Type == CustomClaimTypes.Permission))
                {
                    permissions.Add(claim.Value);
                }
            }
        }

        return permissions;
    }
}

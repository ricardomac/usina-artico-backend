using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<UserResponse>(UserErrors.Unauthorized());
        }

        User? user = await context.Users
            .SingleOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        var roles = await userManager.GetRolesAsync(user);
        var permissions = new HashSet<string>();

        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var claims = await roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    if (claim.Type == "Permission")
                    {
                        permissions.Add(claim.Value);
                    }
                }
            }
        }

        // Also get direct user claims if any
        var userClaims = await userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims)
        {
            if (claim.Type == "Permission")
            {
                permissions.Add(claim.Value);
            }
        }

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Roles = [.. roles],
            Permissions = [.. permissions]
        };
    }
}

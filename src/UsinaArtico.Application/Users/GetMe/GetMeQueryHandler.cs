using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.GetById;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Users.GetMe;

internal sealed class GetMeQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    UserManager<User> userManager,
    IPermissionProvider permissionProvider)
    : IQueryHandler<GetMeQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetMeQuery query, CancellationToken cancellationToken)
    {
        var myId = userContext.UserId;

        User? user = await context.Users
            .SingleOrDefaultAsync(u => u.Id == myId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(myId));
        }

        var roles = await userManager.GetRolesAsync(user);
        var permissions = await permissionProvider.GetForUserIdAsync(user.Id);

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            RoleName = roles.FirstOrDefault() ?? string.Empty,
            Permissions = [.. permissions],
        };
    }
}
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
    IPermissionProvider permissionProvider)
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
        var permissions = await permissionProvider.GetForUserIdAsync(user.Id);

        // var userClaims = await userManager.GetClaimsAsync(user);
        // var nivelAcessoClaim = userClaims.FirstOrDefault(c => c.Type == "nivel_acesso")?.Value;
        // int? nivelAcesso = int.TryParse(nivelAcessoClaim, out var n) ? n : null;

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Roles = [.. roles],
            Permissions = [.. permissions],
            // NivelAcesso = nivelAcesso
        };
    }
}

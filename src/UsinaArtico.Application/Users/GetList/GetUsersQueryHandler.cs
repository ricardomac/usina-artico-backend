using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Users;
using UsinaArtico.SharedKernel;
using System.Linq.Expressions; 

namespace UsinaArtico.Application.Users.GetList;

internal sealed class GetUsersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetUsersQuery, PagedList<UserListResponse>>
{
    public async Task<Result<PagedList<UserListResponse>>> Handle(GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        IQueryable<User> usersQuery = context.Users;

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            usersQuery = usersQuery.Where(p =>
                p.FirstName.Contains(query.SearchTerm) ||
                p.LastName.Contains(query.SearchTerm) ||
                p.Email.Contains(query.SearchTerm));
        }

        usersQuery = OrderBy(query, usersQuery);

        var usersResponseQuery = usersQuery.Select(user => new UserListResponse
        {
            Id = user.Id,
            Nome = user.FirstName + " " + user.LastName,
            Email = user.Email,
            IsActive = user.IsActive,
            NivelAcesso = context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(context.Roles, 
                      ur => ur.RoleId, 
                      r => r.Id, 
                      (ur, r) => r.Name)
                .FirstOrDefault()
        });

        var users = await PagedList<UserListResponse>.CreateAsync(
            usersResponseQuery,
            query.Page,
            query.PageSize);

        return users;
    }

    private static IQueryable<User> OrderBy(GetUsersQuery query, IQueryable<User> usersQuery)
    {
        usersQuery = query.SortOrder?.ToLower() == "desc"
            ? usersQuery.OrderByDescending(GetSortProperty(query))
            : usersQuery.OrderBy(GetSortProperty(query));

        return usersQuery;
    }

    private static Expression<Func<User, object>> GetSortProperty(GetUsersQuery request) =>
        request.SortColumn?.ToLower() switch
        {
            "nome" => cliente => cliente.FirstName,
            _ => cliente => cliente.FirstName
        };
}
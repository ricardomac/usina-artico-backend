using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.GetList;

public sealed record GetUsersQuery(string? SearchTerm,
    bool? IsActive,
    string? Role,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IQuery<PagedList<UserListResponse>>;

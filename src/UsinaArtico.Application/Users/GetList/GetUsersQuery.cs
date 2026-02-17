using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Users.GetList;

public sealed record GetUsersQuery(string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IQuery<PagedList<UserListResponse>>; 

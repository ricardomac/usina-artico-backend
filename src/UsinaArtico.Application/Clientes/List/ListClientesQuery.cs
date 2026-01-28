using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.GetById;

namespace UsinaArtico.Application.Clientes.List;

public sealed record ListClientesQuery(string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IQuery<PagedList<ClienteResponse>>; 
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.GetById;
using UsinaArtico.Domain.Clientes;

namespace UsinaArtico.Application.Clientes.List;

public sealed record ListClientesQuery(string? SearchTerm,
    TipoPessoa? TipoPessoa,
    bool? IsActive,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IQuery<PagedList<ClienteResponse>>;
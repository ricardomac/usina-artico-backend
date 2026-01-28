using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.GetById;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.List;

internal sealed class ListClientesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<ListClientesQuery, PagedList<ClienteResponse>>
{
    public async Task<Result<PagedList<ClienteResponse>>> Handle(ListClientesQuery query,
        CancellationToken cancellationToken)
    {
        IQueryable<Cliente> clientesQuery = context.Clientes;

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            clientesQuery = clientesQuery.Where(p =>
                p.Nome.Contains(query.SearchTerm) ||
                ((string)p.Nome).Contains(query.SearchTerm));
        }

        clientesQuery = OrderBy(query, clientesQuery);

        var clientesResponseQuery = clientesQuery
            .Select(c => new ClienteResponse(
                c.Id,
                c.Nome,
                c.Email.Value,
                c.Telefone,
                c.Tipo == TipoPessoa.Fisica ? (c.Cpf != null ? c.Cpf.Value : "") : (c.Cnpj != null ? c.Cnpj.Value : ""),
                c.CodigoCliente));


        var clientes = await PagedList<ClienteResponse>.CreateAsync(
            clientesResponseQuery,
            query.Page,
            query.PageSize);

        return clientes;
    }

    private static IQueryable<Cliente> OrderBy(ListClientesQuery query, IQueryable<Cliente> clientesQuery)
    {
        clientesQuery = query.SortOrder?.ToLower() == "desc"
            ? clientesQuery.OrderByDescending(GetSortProperty(query))
            : clientesQuery.OrderBy(GetSortProperty(query));

        return clientesQuery;
    }

    private static Expression<Func<Cliente, object>> GetSortProperty(ListClientesQuery request) =>
        request.SortColumn?.ToLower() switch
        {
            "nome" => cliente => cliente.Nome,
            "codigo" => cliente => cliente.CodigoCliente,
            _ => cliente => cliente.Nome
        };
}
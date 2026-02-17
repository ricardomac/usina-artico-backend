using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.GetById; // Reusing response
using UsinaArtico.Application.Clientes.List;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Clientes;

internal sealed class GetList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/clientes", async (
                IQueryHandler<ListClientesQuery, PagedList<ClienteResponse>> handler,
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page = 1,
                int pageSize = 10,
                CancellationToken cancellationToken = default) =>
            {
                var query = new ListClientesQuery(searchTerm, sortColumn, sortOrder, page, pageSize);

                Result<PagedList<ClienteResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.ClientesRead)
            .WithTags(Tags.Clientes)
            .WithSummary("Lista clientes paginados")
            .WithDescription("Retorna uma lista paginada de clientes.")
            .Produces<List<ClienteResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}
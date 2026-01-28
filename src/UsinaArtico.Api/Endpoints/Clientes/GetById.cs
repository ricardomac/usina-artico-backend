using System;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.GetById;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Clientes;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("clientes/{id}", async (
            Guid id,
            IQueryHandler<GetClienteByIdQuery, ClienteResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetClienteByIdQuery(id);

            Result<ClienteResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Clientes)
        .RequireAuthorization()
        .WithSummary("Obtém um cliente pelo ID")
        .WithDescription("Retorna os detalhes de um cliente específico.")
        .Produces<ClienteResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}

using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.GetList;
using UsinaArtico.SharedKernel;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Api.Endpoints.Users;

internal sealed class GetList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (
            IQueryHandler<GetUsersQuery, PagedList<UserListResponse>> handler,
            string? searchTerm,
            bool? isActive,
            string? role,
            string? sortColumn,
            string? sortOrder,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetUsersQuery(searchTerm, isActive, role, sortColumn, sortOrder, page, pageSize);
            Result<PagedList<UserListResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
            
        })
        .HasPermission(Permissions.UsuariosRead)
        .WithTags(Tags.Users)
        .WithSummary("Lista usuários paginados")
        .WithDescription("Retorna uma lista paginada de usuários.")
        .Produces<List<UserListResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}

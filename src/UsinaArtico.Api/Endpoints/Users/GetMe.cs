using UsinaArtico.Api.Endpoints;
using UsinaArtico.Api.Extensions;
using UsinaArtico.Api.Infrastructure;
using UsinaArtico.Application.Abstractions.Authentication;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Users.GetMe;
using UsinaArtico.Application.Users.GetById;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Api.Endpoints.Users;

public sealed class GetMe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/me", async (
            IUserContext userContext,
            IQueryHandler<GetMeQuery, UserResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMeQuery();

            Result<UserResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Users);
    }
}

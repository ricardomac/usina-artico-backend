using UsinaArtico.Domain.Users;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class Auth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/auth")
            .MapIdentityApi<User>()
            .WithTags(Tags.Auth);
    }
}

using Microsoft.AspNetCore.Identity;
using UsinaArtico.Domain.Users;

namespace UsinaArtico.Api.Endpoints.Identity;

public sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async (SignInManager<User> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }) 
        .WithTags(Tags.Auth);
    }
}
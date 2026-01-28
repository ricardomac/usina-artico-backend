using System.Net.Http.Json;
using System.Net.Http.Headers;
using FluentAssertions;
using UsinaArtico.Application.Roles.Get;
using UsinaArtico.Domain.Enums;
using Xunit;

namespace UsinaArtico.IntegrationTests.Roles;

public class RolesEndpointTests : BaseIntegrationTest
{
    public RolesEndpointTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Create_Role_Successfully()
    {
        // Arrange
        await AuthenticateAsync();

        var request = new { Name = "NewRole" };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/roles", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var roleId = await response.Content.ReadFromJsonAsync<Guid>();
        roleId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_List_Roles_Successfully()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync("/api/roles");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var roles = await response.Content.ReadFromJsonAsync<List<RoleResponse>>();
        roles.Should().NotBeNull();
        roles.Should().Contain(r => r.Name == NivelAcesso.Admin.ToString());
        roles.Should().Contain(r => r.Name == NivelAcesso.Vendedor.ToString());
    }

    private async Task AuthenticateAsync()
    {
        // Register and Login to get a token
        var registerRequest = new
        {
            email = "admin@test.com",
            firstName = "Admin",
            lastName = "User",
            password = "Pa$$w0rd!",
            nivelAcesso = (int)NivelAcesso.Admin
        };

        var registerResponse = await HttpClient.PostAsJsonAsync("/api/users/register", registerRequest);
        if (!registerResponse.IsSuccessStatusCode)
        {
             // Maybe already exists from other tests? Try login directly.
        }

        var loginRequest = new
        {
            email = "admin@test.com",
            password = "Pa$$w0rd!"
        };

        var loginResponse = await HttpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse!.AccessToken);
    }

    private record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn);
}

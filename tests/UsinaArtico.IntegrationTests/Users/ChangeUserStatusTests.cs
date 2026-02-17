using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Domain.Users;
using Xunit;

namespace UsinaArtico.IntegrationTests.Users;

public class ChangeUserStatusTests : BaseIntegrationTest
{
    public ChangeUserStatusTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Deactivate_And_Then_Activate_User()
    {
        // Arrange - Register a user
        var email = $"test-{Guid.NewGuid()}@test.com";
        var registerRequest = new
        {
            email = email,
            firstName = "Test",
            lastName = "User",
            password = "Pa$$w0rd!",
            nivelAcesso = 1
        };

        var registerResponse = await HttpClient.PostAsJsonAsync("/api/users/register", registerRequest);
        registerResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var user = await DbContext.Users.FirstAsync(u => u.Email == email);
        user.IsActive.Should().BeTrue();

        // Act - Deactivate
        var deactivateRequest = new { isActive = false };
        var deactivateResponse = await HttpClient.PutAsJsonAsync($"/api/users/{user.Id}/status", deactivateRequest);

        var content = await deactivateResponse.Content.ReadAsStringAsync();
        System.Console.WriteLine($"[DEBUG_LOG] Deactivate Status: {deactivateResponse.StatusCode}");
        System.Console.WriteLine($"[DEBUG_LOG] Deactivate Content: {content}");

        // Assert Deactivate
        deactivateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Refresh from DB
        var deactivatedUser = await DbContext.Users.AsNoTracking().FirstAsync(u => u.Id == user.Id);
        deactivatedUser.IsActive.Should().BeFalse();

        // Act - Activate
        var activateRequest = new { isActive = true };
        var activateResponse = await HttpClient.PutAsJsonAsync($"/api/users/{user.Id}/status", activateRequest);

        // Assert Activate
        activateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Refresh from DB
        var activatedUser = await DbContext.Users.AsNoTracking().FirstAsync(u => u.Id == user.Id);
        activatedUser.IsActive.Should().BeTrue();
    }
}

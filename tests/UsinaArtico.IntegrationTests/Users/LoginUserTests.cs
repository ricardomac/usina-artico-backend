using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace UsinaArtico.IntegrationTests.Users;

public class LoginUserTests : BaseIntegrationTest
{
    public LoginUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Return_Ok_With_Token_When_Credentials_Are_Valid()
    {
        // Arrange
        var registerRequest = new
        {
            email = "test@test.com",
            firstName = "Test",
            lastName = "User",
            password = "Pa$$w0rd!",
            nivelAcesso = 1
        };

        await HttpClient.PostAsJsonAsync("/api/users/register", registerRequest);

        var loginRequest = new
        {
            email = "test@test.com",
            password = "Pa$$w0rd!"
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        tokenResponse.Should().NotBeNull();
        tokenResponse!.AccessToken.Should().NotBeNullOrEmpty();
    }

    private record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn);
}

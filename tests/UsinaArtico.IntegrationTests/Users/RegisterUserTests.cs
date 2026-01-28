using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace UsinaArtico.IntegrationTests.Users;

public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Register_User_Successfully()
    {
        // Arrange
        var request = new 
        {
            email = "newuser@test.com",
            firstName = "Test",
            lastName = "User",
            password = "Pa$$w0rd!",
            nivelAcesso = 1
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/users/register", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UsinaArtico.IntegrationTests.Users;

public class AlterarSenhaTests : BaseIntegrationTest
{
    public AlterarSenhaTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AlterarSenha_ShouldReturnError_WhenCurrentPasswordIsIncorrect()
    {
        // Arrange - Register and Login
        var email = $"test_{Guid.NewGuid()}@test.com";
        var password = "Pa$$w0rd!";
        
        var registerRequest = new
        {
            email,
            firstName = "Test",
            lastName = "User",
            password,
            nivelAcesso = 1
        };

        await HttpClient.PostAsJsonAsync("/api/users/register", registerRequest);

        var loginRequest = new
        {
            email,
            password
        };

        var loginResponse = await HttpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.AccessToken);

        var alterarSenhaRequest = new
        {
            senhaAtual = "WrongPassword!",
            novaSenha = "NewPa$$w0rd!",
            confirmarSenha = "NewPa$$w0rd!"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/users/alterar-senha", alterarSenhaRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Detail.Should().Be("Senha incorreta");
    }

    private record LoginResponse(string AccessToken, string RefreshToken, int ExpiresIn);
}

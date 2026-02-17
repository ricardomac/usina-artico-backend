using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace UsinaArtico.IntegrationTests.Infrastructure;

public class AuthorizationTests : BaseIntegrationTest
{
    public AuthorizationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Return_401_When_Accessing_Protected_Endpoint_Without_Authentication()
    {
        // Act
        // Usamos uma rota que sabemos que existe e está protegida
        HttpResponseMessage response = await HttpClient.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_200_When_Accessing_Protected_Endpoint_With_Authentication()
    {
        // Arrange
        var registerRequest = new
        {
            email = "auth_test@test.com",
            firstName = "Auth",
            lastName = "Test",
            password = "Pa$$w0rd!",
            nivelAcesso = 1 // Admin
        };

        // Registrar usuário (pode precisar de permissão se o seeder não rodou, mas em testes costuma ser aberto ou pré-configurado)
        // Como o seeder roda no Program.cs em Development, o Admin padrão deve existir se usarmos as credenciais dele.
        // Mas vamos criar um novo e logar.
        
        await HttpClient.PostAsJsonAsync("/api/users/register", registerRequest);

        var loginRequest = new
        {
            email = "auth_test@test.com",
            password = "Pa$$w0rd!"
        };

        var loginResponse = await HttpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        
        HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.AccessToken);

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private record LoginResponse(string AccessToken);
}

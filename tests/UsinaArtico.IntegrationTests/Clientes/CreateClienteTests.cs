using System.Net.Http.Json;
using UsinaArtico.Api.Endpoints.Clientes;
using UsinaArtico.Application.Clientes.GetById;

namespace UsinaArtico.IntegrationTests.Clientes;

public class CreateClienteTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldCreateCliente_WhenRequestIsValid()
    {
        // Arrange
        var request = new
        {
            Nome = "Cliente Teste",
            Email = "teste@cliente.com",
            Telefone = "11999999999",
            Documento = "12345678901",
            CodigoCliente = "CLI-001",
            Enderecos = new List<object>() // Empty list or populated if needed, Command expects List<CreateEnderecoCommand>
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/clientes", request);

        // Assert
        response.EnsureSuccessStatusCode();

        var clienteId = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, clienteId);
    }
}

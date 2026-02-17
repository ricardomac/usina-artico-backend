using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.IntegrationTests.Clientes;

public class ChangeStatusTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task ChangeStatus_ShouldUpdateIsActive_WhenRequestIsValid()
    {
        // Arrange
        var cliente = Cliente.Create(
            "Cliente Teste Status",
            Email.Create("status@teste.com").Value,
            "11988887777",
            "CLI-STATUS",
            TipoPessoa.Fisica,
            Cpf.Create("12345678901").Value,
            null,
            true
        );

        DbContext.Clientes.Add(cliente);
        await DbContext.SaveChangesAsync();

        var request = new { IsActive = false };

        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/clientes/{cliente.Id}/status", request);

        // Assert
        response.EnsureSuccessStatusCode();

        // Verificar no banco de dados
        var updatedCliente = await DbContext.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == cliente.Id);

        Assert.NotNull(updatedCliente);
        Assert.False(updatedCliente.IsActive);
    }
}

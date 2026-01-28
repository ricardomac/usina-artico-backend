using Microsoft.Extensions.DependencyInjection;
using UsinaArtico.Infrastructure.Database;
using Xunit;

namespace UsinaArtico.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly IServiceScope _scope;
    protected readonly ApplicationDbContext DbContext;
    protected readonly HttpClient HttpClient;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        HttpClient = factory.CreateClient();
    }
}

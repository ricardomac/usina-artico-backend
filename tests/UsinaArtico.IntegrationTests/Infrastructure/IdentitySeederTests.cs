using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UsinaArtico.Domain.Enums;
using Xunit;

namespace UsinaArtico.IntegrationTests.Infrastructure;

public class IdentitySeederTests : BaseIntegrationTest
{
    public IdentitySeederTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_Seed_Default_Roles_On_Startup()
    {
        // Act - Startup Logic runs on factory creation

        // Assert
        using var scope = _scope.ServiceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var rolesStr = new[] { NivelAcesso.Admin.ToString(), NivelAcesso.Vendedor.ToString(), NivelAcesso.Usuario.ToString() };

        foreach (var roleName in rolesStr)
        {
            // Verify Role Exists
            var exists = await roleManager.RoleExistsAsync(roleName);
            Assert.True(exists, $"Role {roleName} should exist.");

            // Verify Claims (Optional - checking Admin claims)
            if (roleName == NivelAcesso.Admin.ToString())
            {
                var role = await roleManager.FindByNameAsync(roleName);
                var claims = await roleManager.GetClaimsAsync(role!);
                Assert.NotEmpty(claims);
            }
        }
    }
}

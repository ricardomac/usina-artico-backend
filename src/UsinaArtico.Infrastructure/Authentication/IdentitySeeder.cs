using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using UsinaArtico.Domain.Enums;
using UsinaArtico.SharedKernel.Authorization;

namespace UsinaArtico.Infrastructure.Authentication;

public sealed class IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager)
{
    public async Task SeedAsync()
    {
        await SeedRoleAsync(NivelAcesso.Admin, 
        [
            new Claim("Permission", Permissoes.Admin),
            new Claim("Permission", Permissoes.Produtos.Ler),
            new Claim("Permission", Permissoes.Produtos.Escrever),
            new Claim("Permission", Permissoes.Vendedor),
            new Claim("Permission", Permissoes.Usuario)
        ]);

        await SeedRoleAsync(NivelAcesso.Vendedor, 
        [
            new Claim("Permission", Permissoes.Vendedor),
            new Claim("Permission", Permissoes.Produtos.Ler)
        ]);

        await SeedRoleAsync(NivelAcesso.Usuario, 
        [
            new Claim("Permission", Permissoes.Usuario)
        ]);
    }

    private async Task SeedRoleAsync(NivelAcesso roleEnum, IEnumerable<Claim> claims)
    {
        string roleName = roleEnum.ToString();

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole<Guid>(roleName);
            await roleManager.CreateAsync(role);
        }
        
        var existingRole = await roleManager.FindByNameAsync(roleName);
        if (existingRole is null) return;

        var existingClaims = await roleManager.GetClaimsAsync(existingRole);
        
        foreach (var claim in claims)
        {
            if (!existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                await roleManager.AddClaimAsync(existingRole, claim);
            }
        }
    }
}

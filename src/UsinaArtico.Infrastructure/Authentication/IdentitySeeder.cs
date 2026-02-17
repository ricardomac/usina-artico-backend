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
            new Claim(CustomClaimTypes.Permission, Permissions.UsuariosRead),
            new Claim(CustomClaimTypes.Permission, Permissions.UsuariosWrite),
            new Claim(CustomClaimTypes.Permission, Permissions.UsuariosUpdate),
            new Claim(CustomClaimTypes.Permission, Permissions.UsuariosDelete),
            
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesRead),
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesWrite),
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesUpdate),
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesDelete),
            
               
            new Claim(CustomClaimTypes.Permission, Permissions.GestaoFaturasRead),
            new Claim(CustomClaimTypes.Permission, Permissions.GestaoFaturasWrite),
            new Claim(CustomClaimTypes.Permission, Permissions.GestaoFaturasUpdate),
            new Claim(CustomClaimTypes.Permission, Permissions.GestaoFaturasDelete),
            
        ]);

        await SeedRoleAsync(NivelAcesso.Vendedor, 
        [
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesRead),
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesWrite),
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesUpdate),
            new Claim(CustomClaimTypes.Permission, Permissions.ClientesDelete),
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

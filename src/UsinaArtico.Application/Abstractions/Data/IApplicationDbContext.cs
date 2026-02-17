using Microsoft.AspNetCore.Identity; // Necessário para IdentityRole e IdentityUserRole
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UsinaArtico.Domain.Todos;
using UsinaArtico.Domain.Users;

namespace UsinaArtico.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    
    DbSet<IdentityRole<Guid>> Roles { get; }
    DbSet<IdentityUserRole<Guid>> UserRoles { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Domain.Estados.Estado> Estados { get; }
    DbSet<Domain.Cidades.Cidade> Cidades { get; }
    DbSet<Domain.Clientes.Cliente> Clientes { get; }
    DbSet<Domain.Contratos.Contrato> Contratos { get; }
    DbSet<Domain.Enderecos.Endereco> Enderecos { get; }

    DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; }
    
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
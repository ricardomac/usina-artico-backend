using Microsoft.EntityFrameworkCore;
using UsinaArtico.Domain.Todos;
using UsinaArtico.Domain.Users;

namespace UsinaArtico.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }

    DbSet<Domain.Estados.Estado> Estados { get; }
    DbSet<Domain.Cidades.Cidade> Cidades { get; }
    DbSet<Domain.Clientes.Cliente> Clientes { get; }
    DbSet<Domain.Contratos.Contrato> Contratos { get; }
    DbSet<Domain.Enderecos.Endereco> Enderecos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

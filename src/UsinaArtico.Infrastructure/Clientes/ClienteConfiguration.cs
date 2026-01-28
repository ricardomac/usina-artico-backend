using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Infrastructure.Clientes;

internal sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).HasMaxLength(200).IsRequired();
        
        builder.Property(c => c.Email)
            .HasConversion(email => email != null ? email.Value : null, value => value != null ? Email.Create(value).Value : null)
            .HasMaxLength(200);
            
        builder.Property(c => c.Telefone).HasMaxLength(20);
        builder.Property(c => c.CodigoCliente).HasMaxLength(50);

        // Native Enum mapping (no conversion)
        builder.Property(c => c.Tipo)
            .IsRequired();

        builder.Property(c => c.Cpf)
            .HasConversion(cpf => cpf != null ? cpf.Value : null, value => value != null ? Cpf.Create(value).Value : null) 
            .HasMaxLength(11);
            
        builder.Property(c => c.Cnpj)
             .HasConversion(cnpj => cnpj != null ? cnpj.Value : null, value => value != null ? Cnpj.Create(value).Value : null)
             .HasMaxLength(14);
        
        builder.HasIndex(c => c.CodigoCliente);
        
        builder.HasMany(c => c.Enderecos)
            .WithOne(e => e.Cliente)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsinaArtico.Domain.Estados;

namespace UsinaArtico.Infrastructure.Estados;

internal sealed class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Nome).HasMaxLength(100).IsRequired();
        
        builder.Property(e => e.UF)
            .HasConversion(uf => uf.Value, value => Uf.Create(value))
            .HasMaxLength(2)
            .IsRequired();

        builder.HasMany(e => e.Cidades)
            .WithOne(c => c.Estado)
            .HasForeignKey(c => c.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

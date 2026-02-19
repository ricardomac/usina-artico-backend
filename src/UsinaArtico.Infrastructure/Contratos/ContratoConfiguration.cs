using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsinaArtico.Domain.Contratos;

namespace UsinaArtico.Infrastructure.Contratos;

internal sealed class ContratoConfiguration : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Anexo).HasMaxLength(500); 
        builder.Property(c => c.ValorKwh).HasColumnType("decimal(18,2)");
        builder.Property(c => c.QuantidadeKwh).HasColumnType("decimal(18,2)");
        builder.Property(c => c.DataInicio).HasColumnType("date");
        
        builder.HasMany(c => c.Enderecos)
            .WithOne(e => e.Contrato)
            .HasForeignKey(e => e.ContratoId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}

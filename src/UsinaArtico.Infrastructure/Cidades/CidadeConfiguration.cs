using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsinaArtico.Domain.Cidades;

namespace UsinaArtico.Infrastructure.Cidades;

internal sealed class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
{
    public void Configure(EntityTypeBuilder<Cidade> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Nome).HasMaxLength(150).IsRequired();
    }
}

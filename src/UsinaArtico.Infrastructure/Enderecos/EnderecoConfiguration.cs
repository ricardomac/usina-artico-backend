using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsinaArtico.Domain.Enderecos;

namespace UsinaArtico.Infrastructure.Enderecos;

internal sealed class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CodigoInstalacao).HasMaxLength(50);
        builder.Property(e => e.Logradouro).HasMaxLength(200).IsRequired();
        
        // Native Enum mapping (no conversion)
        builder.Property(e => e.TipoLigacao)
            .IsRequired();
            
        builder.Property(e => e.Cep).HasMaxLength(10).IsRequired();
        builder.Property(e => e.Numero).HasMaxLength(20);
        builder.Property(e => e.Bairro).HasMaxLength(100);

        builder.HasOne(e => e.Cidade)
            .WithMany()
            .HasForeignKey(e => e.CidadeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using UsinaArtico.Domain.Enderecos;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Contratos;

public sealed class Contrato : Entity
{
    public Guid Id { get; set; }
    public string Anexo { get; set; } = string.Empty;
    public decimal ValorKwh { get; set; }
    public decimal QuantidadeKwh { get; set; }
    public DateTime DataVencimento { get; set; }

    public List<Endereco> Enderecos { get; set; } = [];
}

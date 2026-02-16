using UsinaArtico.Domain.Estados;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Cidades;

public sealed class Cidade : Entity
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid EstadoId { get; set; }
    
    public Estado Estado { get; set; }
}

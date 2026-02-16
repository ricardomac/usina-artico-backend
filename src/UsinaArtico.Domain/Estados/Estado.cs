using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Estados;

public sealed class Estado : Entity
{
    public Guid Id { get; set; }
    public Uf UF { get; set; } 
    public string Nome { get; set; } = string.Empty;

    public List<Cidades.Cidade> Cidades { get; set; } = [];
}

using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Estados;

public sealed class Uf : ValueObject
{
    public string Value { get; }

    private Uf(string value)
    {
        Value = value;
    }

    public static Uf Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 2)
        {
            throw new ArgumentException("UF inválida", nameof(value));
        }
        
        // Could validate list of valid UFs here

        return new Uf(value.ToUpper());
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

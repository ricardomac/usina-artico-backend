using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Clientes;

public sealed class Cnpj : ValueObject
{
    public string Value { get; }

    private Cnpj(string value)
    {
        Value = value;
    }

    public static Result<Cnpj> Create(string value)
    {
         if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Cnpj>(Error.Failure("Cnpj.Empty", "CNPJ cannot be empty"));
        }

        var digitsOnly = new string(value.Where(char.IsDigit).ToArray());

        if (digitsOnly.Length != 14)
        {
            return Result.Failure<Cnpj>(Error.Failure("Cnpj.InvalidLength", "CNPJ must have 14 digits"));
        }

        return new Cnpj(digitsOnly);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

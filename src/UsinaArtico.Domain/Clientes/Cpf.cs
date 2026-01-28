using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Clientes;

public sealed class Cpf : ValueObject
{
    public string Value { get; }

    private Cpf(string value)
    {
        Value = value;
    }

    public static Result<Cpf> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Cpf>(Error.Failure("Cpf.Empty", "CPF cannot be empty"));
        }
        
        var digitsOnly = new string(value.Where(char.IsDigit).ToArray());

        if (digitsOnly.Length != 11)
        {
             return Result.Failure<Cpf>(Error.Failure("Cpf.InvalidLength", "CPF must have 11 digits"));
        }
        
        // Add algorhitm validation if needed
        
        return new Cpf(digitsOnly);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

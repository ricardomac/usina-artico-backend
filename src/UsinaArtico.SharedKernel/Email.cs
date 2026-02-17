using System.Text.RegularExpressions;

namespace UsinaArtico.SharedKernel;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static implicit operator string(Email email) => email.Value;

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(Error.Failure("Email.Empty", "O e-mail não pode ser vazio."));
        }

        if (email.Length > 200)
        {
            return Result.Failure<Email>(Error.Failure("Email.TooLong", "O e-mail deve ter no máximo 200 caracteres."));
        }

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250)))
        {
            return Result.Failure<Email>(Error.Failure("Email.InvalidFormat", "O e-mail informado não é válido."));
        }

        return new Email(email);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
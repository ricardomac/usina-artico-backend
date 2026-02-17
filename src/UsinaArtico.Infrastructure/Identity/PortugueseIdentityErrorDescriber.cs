using Microsoft.AspNetCore.Identity;

namespace UsinaArtico.Infrastructure.Identity;

public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateUserName(string userName) 
        => new() { Code = nameof(DuplicateUserName), Description = "Já existe uma conta com este email" };

    public override IdentityError DuplicateEmail(string email) 
        => new() { Code = nameof(DuplicateEmail), Description = "Já existe uma conta com este email" };

    public override IdentityError InvalidEmail(string email) 
        => new() { Code = nameof(InvalidEmail), Description = $"O email '{email}' é inválido" };

    public override IdentityError PasswordTooShort(int length) 
        => new() { Code = nameof(PasswordTooShort), Description = $"A senha deve ter no mínimo {length} caracteres" };

    public override IdentityError PasswordRequiresNonAlphanumeric() 
        => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter pelo menos um caractere especial" };

    public override IdentityError PasswordRequiresDigit() 
        => new() { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter pelo menos um número ('0'-'9')" };

    public override IdentityError PasswordRequiresLower() 
        => new() { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter pelo menos uma letra minúscula ('a'-'z')" };

    public override IdentityError PasswordRequiresUpper() 
        => new() { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter pelo menos uma letra maiúscula ('A'-'Z')" };
}

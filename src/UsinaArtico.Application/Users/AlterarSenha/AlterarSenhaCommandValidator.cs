using FluentValidation;

namespace UsinaArtico.Application.Users.AlterarSenha;

public sealed class AlterarSenhaCommandValidator : AbstractValidator<AlterarSenhaCommand>
{
    public AlterarSenhaCommandValidator()
    {
        RuleFor(c => c.SenhaAtual)
            .NotEmpty()
            .WithMessage("A senha atual é obrigatória.");

        RuleFor(c => c.NovaSenha)
            .NotEmpty()
            .WithMessage("A nova senha é obrigatória.")
            .MinimumLength(6)
            .WithMessage("A nova senha deve ter no mínimo 6 caracteres.");

        RuleFor(c => c.ConfirmarSenha)
            .Equal(c => c.NovaSenha)
            .WithMessage("A confirmação de senha não confere com a nova senha.");
    }
}

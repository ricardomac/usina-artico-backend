using FluentValidation;
using UsinaArtico.Domain.Clientes;

namespace UsinaArtico.Application.Clientes.Create;

public sealed class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
{
    public CreateClienteCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("O email é obrigatório.");
        
        RuleFor(c => c.Documento)
            .NotEmpty().WithMessage("O documento (CPF/CNPJ) é obrigatório.")
            .MaximumLength(20).WithMessage("O documento deve ter no máximo 20 caracteres.")
            .Matches(@"^[\d.\-/]+$").WithMessage("O documento contém caracteres inválidos.")
            .When(c => !string.IsNullOrWhiteSpace(c.Documento)); // Proteção extra
        
        RuleFor(c => c.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");
        
        RuleFor(c => c.CodigoCliente)
            .NotEmpty().WithMessage("O Codigo do cliente é obrigatório")
            .MaximumLength(50).WithMessage("O código do cliente deve ter no máximo 50 caracteres.")
            .When(c => !string.IsNullOrWhiteSpace(c.CodigoCliente));
        
        RuleFor(c => c.Tipo)
            .IsInEnum().WithMessage("O tipo de pessoa informado é inválido.")
            .Must(tipo => tipo == TipoPessoa.Fisica || tipo == TipoPessoa.Juridica)
            .WithMessage("O tipo de pessoa deve ser Física (1) ou Jurídica (2).");
    }
}

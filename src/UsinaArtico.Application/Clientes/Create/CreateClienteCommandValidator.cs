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
            .Must(d =>
            {
                var digits = d.Where(char.IsDigit).Count();
                return digits == 11 || digits == 14;
            }).WithMessage("O documento deve ter 11 (CPF) ou 14 (CNPJ) dÃ­gitos.")
            .When(c => !string.IsNullOrWhiteSpace(c.Documento));

        RuleFor(c => c.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

        RuleFor(c => c.CodigoCliente)
            .NotEmpty().WithMessage("O Codigo do cliente é obrigatório")
            .MaximumLength(50).WithMessage("O código do cliente deve ter no máximo 50 caracteres.")
            .When(c => !string.IsNullOrWhiteSpace(c.CodigoCliente));



        RuleForEach(c => c.Enderecos).SetValidator(new CreateEnderecoCommandValidator());
    }
}

public sealed class CreateEnderecoCommandValidator : AbstractValidator<EnderecoDto>
{
    public CreateEnderecoCommandValidator()
    {
        RuleFor(e => e.Logradouro).NotEmpty().MaximumLength(200);
        RuleFor(e => e.Cep).NotEmpty().MaximumLength(10);
        RuleFor(e => e.Numero).MaximumLength(20);
        RuleFor(e => e.Bairro).MaximumLength(100);
        RuleFor(e => e.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(e => e.Uf).NotEmpty().Length(2);

        When(e => e.Contrato != null, () =>
        {
            RuleFor(e => e.Contrato!).SetValidator(new CreateContratoCommandValidator());
        });
    }
}

public sealed class CreateContratoCommandValidator : AbstractValidator<ContratoDto>
{
    public CreateContratoCommandValidator()
    {
        RuleFor(c => c.ValorKwh).GreaterThan(0);
        RuleFor(c => c.QuantidadeKwh).GreaterThan(0);
        RuleFor(c => c.DataInicio).NotEmpty();
    }
}

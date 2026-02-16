using FluentValidation;
using UsinaArtico.Domain.Clientes;

namespace UsinaArtico.Application.Clientes.Update;

public sealed class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
{
    public UpdateClienteCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Nome).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.CodigoCliente).NotEmpty().MaximumLength(50);
        RuleFor(c => c.Documento)
            .NotEmpty().WithMessage("O documento (CPF/CNPJ) é obrigatório.")
            .Must(d =>
            {
                var digits = d.Where(char.IsDigit).Count();
                return digits == 11 || digits == 14;
            }).WithMessage("O documento deve ter 11 (CPF) ou 14 (CNPJ) dÃ­gitos.");

        RuleForEach(c => c.Enderecos).SetValidator(new UpdateEnderecoCommandValidator());
    }
}

public sealed class UpdateEnderecoCommandValidator : AbstractValidator<UpdateEnderecoCommand>
{
    public UpdateEnderecoCommandValidator()
    {
        RuleFor(e => e.Logradouro).NotEmpty().MaximumLength(200);
        RuleFor(e => e.Cep).NotEmpty().MaximumLength(10);
        RuleFor(e => e.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(e => e.Uf).NotEmpty().Length(2);

        When(e => e.Contrato != null, () =>
        {
            RuleFor(e => e.Contrato!).SetValidator(new UpdateContratoCommandValidator());
        });
    }
}

public sealed class UpdateContratoCommandValidator : AbstractValidator<UpdateContratoCommand>
{
    public UpdateContratoCommandValidator()
    {
        RuleFor(c => c.ValorKwh).GreaterThan(0);
        RuleFor(c => c.QuantidadeKwh).GreaterThan(0);
        RuleFor(c => c.DataInicio).NotEmpty();
    }
}

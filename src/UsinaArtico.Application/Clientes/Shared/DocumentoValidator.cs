using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.Shared;

/// <summary>
/// Classe utilitária para validação e identificação de documentos (CPF/CNPJ).
/// </summary>
public static class DocumentoValidator
{
    /// <summary>
    /// Valida o documento e identifica automaticamente o tipo de pessoa.
    /// CPF (11 dígitos) = Pessoa Física
    /// CNPJ (14 dígitos) = Pessoa Jurídica
    /// </summary>
    /// <param name="documento">Documento a ser validado (pode conter formatação)</param>
    /// <returns>Result contendo o tipo de pessoa e os value objects CPF ou CNPJ</returns>
    public static Result<(TipoPessoa Tipo, Cpf? Cpf, Cnpj? Cnpj)> ValidarEIdentificarTipo(string documento)
    {
        // Remove caracteres não numéricos
        var documentoDigits = new string(documento.Where(char.IsDigit).ToArray());

        // Identifica o tipo baseado na quantidade de dígitos
        if (documentoDigits.Length == 11)
        {
            // CPF - Pessoa Física
            var cpfResult = Cpf.Create(documentoDigits);
            if (cpfResult.IsFailure)
            {
                return Result.Failure<(TipoPessoa, Cpf?, Cnpj?)>(cpfResult.Error);
            }

            return Result.Success((TipoPessoa.Fisica, cpfResult.Value, (Cnpj?)null));
        }
        else if (documentoDigits.Length == 14)
        {
            // CNPJ - Pessoa Jurídica
            var cnpjResult = Cnpj.Create(documentoDigits);
            if (cnpjResult.IsFailure)
            {
                return Result.Failure<(TipoPessoa, Cpf?, Cnpj?)>(cnpjResult.Error);
            }

            return Result.Success((TipoPessoa.Juridica, (Cpf?)null, cnpjResult.Value));
        }
        else
        {
            // Documento inválido
            return Result.Failure<(TipoPessoa, Cpf?, Cnpj?)>(ClienteErrors.InvalidDocument);
        }
    }
}

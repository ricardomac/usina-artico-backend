using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Enderecos;

public static class EnderecoErrors
{
    public static Error DuplicateCodigoInstalacao => Error.Conflict(
        "Endereco.DuplicateCodigoInstalacao",
        "Já existe um endereço com este Código de Instalação.");
}

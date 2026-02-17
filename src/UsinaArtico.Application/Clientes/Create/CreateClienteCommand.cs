using UsinaArtico.Application.Abstractions.Messaging;

namespace UsinaArtico.Application.Clientes.Create;

public sealed record CreateClienteCommand(
    string Nome,
    string Email,
    string Telefone,
    string Documento,
    string CodigoCliente,
    bool IsActive,
    List<EnderecoDto> Enderecos) : ICommand<Guid>;

public sealed record EnderecoDto(
    string CodigoInstalacao,
    string Logradouro,
    int TipoLigacao,
    string Cep,
    string Numero,
    string Bairro,
    string Cidade,
    string Uf,
    ContratoDto? Contrato);

public sealed record ContratoDto(
    decimal ValorKwh,
    decimal QuantidadeKwh,
    DateTime DataInicio,
    string Anexo);

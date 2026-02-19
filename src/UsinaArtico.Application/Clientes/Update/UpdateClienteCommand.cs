using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Application.Clientes.Create;
using UsinaArtico.Domain.Clientes;

namespace UsinaArtico.Application.Clientes.Update;

public sealed record UpdateClienteCommand(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Documento,
    string CodigoCliente,
    List<UpdateEnderecoCommand> Enderecos) : ICommand;

public sealed record UpdateEnderecoCommand(
    Guid? Id,
    string CodigoInstalacao,
    string Logradouro,
    int TipoLigacao,
    string Cep,
    string Numero,
    string Bairro,
    string Cidade,
    string Uf,
    UpdateContratoCommand? Contrato);

public sealed record UpdateContratoCommand(
    Guid? Id,
    decimal ValorKwh,
    decimal QuantidadeKwh,
    DateTime DataInicio,
    string Anexo);

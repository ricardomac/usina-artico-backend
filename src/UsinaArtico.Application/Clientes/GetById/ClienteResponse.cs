namespace UsinaArtico.Application.Clientes.GetById;

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string Documento,
    string CodigoCliente,
    List<EnderecoResponse> Enderecos);

public sealed record EnderecoResponse(
    Guid Id,
    string CodigoInstalacao,
    string Logradouro,
    int TipoLigacao,
    string Cep,
    string Numero,
    string Bairro,
    string Cidade,
    string Uf,
    ContratoResponse? Contrato = null);

public sealed record ContratoResponse(
    Guid Id,
    decimal ValorKwh,
    decimal QuantidadeKwh,
    DateTime DataInicio,
    string Anexo);

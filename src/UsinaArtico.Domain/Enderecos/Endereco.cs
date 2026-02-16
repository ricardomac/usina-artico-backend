using UsinaArtico.Domain.Cidades;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.Domain.Contratos;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Enderecos;

public sealed class Endereco : Entity
{
    private Endereco() { }

    public Guid Id { get; private set; }
    public string CodigoInstalacao { get; private set; } = string.Empty;
    public string Logradouro { get; private set; } = string.Empty;
    public TipoLigacao TipoLigacao { get; private set; }
    public string Cep { get; private set; } = string.Empty;
    public string Numero { get; private set; } = string.Empty;
    public string Bairro { get; private set; } = string.Empty;

    public Guid CidadeId { get; private set; }
    public Cidade Cidade { get; private set; }

    public Guid ClienteId { get; private set; }
    public Cliente Cliente { get; private set; }

    public Guid? ContratoId { get; private set; }
    public Contrato? Contrato { get; private set; }

    public static Endereco Create(
        string codigoInstalacao,
        string logradouro,
        TipoLigacao tipoLigacao,
        string cep,
        string numero,
        string bairro,
        Guid cidadeId,
        Guid clienteId,
        Guid? contratoId = null)
    {
        return new Endereco
        {
            Id = Guid.NewGuid(),
            CodigoInstalacao = codigoInstalacao,
            Logradouro = logradouro,
            TipoLigacao = tipoLigacao,
            Cep = cep,
            Numero = numero,
            Bairro = bairro,
            CidadeId = cidadeId,
            ClienteId = clienteId,
            ContratoId = contratoId
        };
    }

    public void Update(
        string codigoInstalacao,
        string logradouro,
        TipoLigacao tipoLigacao,
        string cep,
        string numero,
        string bairro,
        Guid cidadeId,
        Guid? contratoId)
    {
        CodigoInstalacao = codigoInstalacao;
        Logradouro = logradouro;
        TipoLigacao = tipoLigacao;
        Cep = cep;
        Numero = numero;
        Bairro = bairro;
        CidadeId = cidadeId;
        ContratoId = contratoId;
    }
}

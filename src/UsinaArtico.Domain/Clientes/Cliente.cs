using UsinaArtico.Domain.Enderecos;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Domain.Clientes;

public sealed class Cliente : Entity
{
    private Cliente() { }

    private Cliente(
        Guid id,
        string nome,
        Email email,
        string telefone,
        string codigoCliente,
        TipoPessoa tipo,
        Cpf? cpf,
        Cnpj? cnpj,
        bool isActive = true)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        CodigoCliente = codigoCliente;
        Tipo = tipo;
        Cpf = cpf;
        Cnpj = cnpj;
        IsActive = isActive;
    }

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public Email Email { get; private set; }
    public string Telefone { get; private set; } = string.Empty;
    public string CodigoCliente { get; private set; } = string.Empty;

    public TipoPessoa Tipo { get; private set; }
    public Cpf? Cpf { get; private set; }
    public Cnpj? Cnpj { get; private set; }
    public bool IsActive { get; private set; }

    public List<Endereco> Enderecos { get; private set; } = [];

    public string Documento => Tipo == TipoPessoa.Fisica ? Cpf?.Value ?? "" : Cnpj?.Value ?? "";

    public static Cliente Create(
        string nome,
        Email email,
        string telefone,
        string codigoCliente,
        TipoPessoa tipo,
        Cpf? cpf,
        Cnpj? cnpj,
        bool isActive = true)
    {

        return new Cliente(Guid.NewGuid(), nome, email, telefone, codigoCliente, tipo, cpf, cnpj, isActive);
    }

    public void Update(
        string nome,
        Email email,
        string telefone,
        string codigoCliente,
        TipoPessoa tipo,
        Cpf? cpf,
        Cnpj? cnpj,
        bool isActive)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        CodigoCliente = codigoCliente;
        Tipo = tipo;
        Cpf = cpf;
        Cnpj = cnpj;
        IsActive = isActive;
    }

    public void SetStatus(bool isActive)
    {
        IsActive = isActive;
    }
}

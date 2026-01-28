using System.Security.Claims;

namespace UsinaArtico.SharedKernel.Authorization;

public static class Permissoes
{
    public const string Admin = "Permissoes.Admin";
    public const string Vendedor = "Permissoes.Vendedor";
    public const string Usuario = "Permissoes.Usuario";

    public static class Produtos
    {
        public const string Ler = "Permissoes.Produtos.Ler";
        public const string Escrever = "Permissoes.Produtos.Escrever";
    }
}

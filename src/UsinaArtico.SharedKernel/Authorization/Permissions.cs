using System.Security.Claims;

namespace UsinaArtico.SharedKernel.Authorization;

public static class Permissions
{
    public const string UsuariosRead = "usuarios:read";
    public const string UsuariosWrite = "usuarios:write";
    public const string UsuariosUpdate = "usuarios:update";
    public const string UsuariosDelete = "usuarios:delete";
    
    public const string ClientesRead = "clientes:read";
    public const string ClientesWrite = "clientes:write";
    public const string ClientesUpdate = "clientes:update";
    public const string ClientesDelete = "clientes:delete";
    
    public const string GestaoFaturasRead = "gestao-faturas:read";
    public const string GestaoFaturasWrite = "gestao-faturas:write";
    public const string GestaoFaturasUpdate = "gestao-faturas:update";
    public const string GestaoFaturasDelete = "gestao-faturas:delete";
    
    public const string TodosRead = "todos:read";
    public const string TodosWrite = "todos:write";
    public const string TodosUpdate = "todos:update";
    public const string TodosDelete = "todos:delete";

}
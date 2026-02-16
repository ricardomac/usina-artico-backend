using UsinaArtico.Domain.Cidades;

namespace UsinaArtico.Application.Abstractions.Services;

/// <summary>
/// Serviço responsável por resolver (buscar ou criar) cidades e estados.
/// </summary>
public interface ICidadeResolverService
{
    /// <summary>
    /// Resolve ou cria uma cidade com base no nome e UF fornecidos.
    /// Busca primeiro no banco de dados, depois no cache local do EF Core.
    /// Se não encontrar, cria o estado (se necessário) e a cidade.
    /// </summary>
    /// <param name="nomeCidade">Nome da cidade</param>
    /// <param name="uf">Sigla do estado (UF)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Cidade encontrada ou criada</returns>
    Task<Cidade> ResolverOuCriarCidadeAsync(
        string nomeCidade,
        string uf,
        CancellationToken cancellationToken);
}

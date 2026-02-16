using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Services;
using UsinaArtico.Domain.Cidades;
using UsinaArtico.Domain.Estados;

namespace UsinaArtico.Application.Services;

/// <summary>
/// Implementação do serviço de resolução de cidades.
/// Encapsula a lógica de busca e criação de cidades e estados.
/// </summary>
internal sealed class CidadeResolverService(IApplicationDbContext context) : ICidadeResolverService
{
    public async Task<Cidade> ResolverOuCriarCidadeAsync(
        string nomeCidade,
        string uf,
        CancellationToken cancellationToken)
    {
        // 1. Resolver Estado (buscar no banco ou criar)
        var ufValueObject = Uf.Create(uf);
        var estado = await BuscarOuCriarEstadoAsync(ufValueObject, cancellationToken);

        // 2. Resolver Cidade (buscar no banco ou criar)
        var cidadeNome = nomeCidade.Trim();
        var cidade = await BuscarOuCriarCidadeAsync(cidadeNome, estado, cancellationToken);

        return cidade;
    }

    /// <summary>
    /// Busca o estado no banco de dados ou no cache local do EF Core.
    /// Se não encontrar, cria um novo estado.
    /// </summary>
    private async Task<Estado> BuscarOuCriarEstadoAsync(
        Uf uf,
        CancellationToken cancellationToken)
    {
        // Buscar no banco de dados
        var estado = await context.Estados
            .FirstOrDefaultAsync(e => e.UF == uf, cancellationToken);

        if (estado is not null)
        {
            return estado;
        }

        // Buscar no cache local do EF Core (entidades adicionadas mas ainda não salvas)
        estado = context.Estados.Local
            .FirstOrDefault(e => e.UF.Equals(uf));

        if (estado is not null)
        {
            return estado;
        }

        // Criar novo estado
        estado = new Estado
        {
            Id = Guid.NewGuid(),
            UF = uf,
            Nome = uf.Value // Fallback: usar a sigla como nome
        };

        context.Estados.Add(estado);

        return estado;
    }

    /// <summary>
    /// Busca a cidade no banco de dados ou no cache local do EF Core.
    /// Se não encontrar, cria uma nova cidade.
    /// </summary>
    private async Task<Cidade> BuscarOuCriarCidadeAsync(
        string nomeCidade,
        Estado estado,
        CancellationToken cancellationToken)
    {
        // Buscar no banco de dados
        var cidade = await context.Cidades
            .FirstOrDefaultAsync(
                c => c.Nome.ToLower() == nomeCidade.ToLower() && c.EstadoId == estado.Id,
                cancellationToken);

        if (cidade is not null)
        {
            return cidade;
        }

        // Buscar no cache local do EF Core (entidades adicionadas mas ainda não salvas)
        cidade = context.Cidades.Local
            .FirstOrDefault(c =>
                c.Nome.Equals(nomeCidade, StringComparison.CurrentCultureIgnoreCase) &&
                (c.EstadoId == estado.Id || c.Estado == estado));

        if (cidade is not null)
        {
            return cidade;
        }

        // Criar nova cidade
        cidade = new Cidade
        {
            Id = Guid.NewGuid(),
            Nome = nomeCidade,
            Estado = estado,
            EstadoId = estado.Id
        };

        context.Cidades.Add(cidade);

        return cidade;
    }
}

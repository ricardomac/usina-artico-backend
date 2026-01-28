using Microsoft.EntityFrameworkCore;
using UsinaArtico.Application.Abstractions.Data;
using UsinaArtico.Application.Abstractions.Messaging;
using UsinaArtico.Domain.Clientes;
using UsinaArtico.SharedKernel;

namespace UsinaArtico.Application.Clientes.GetById;

internal sealed class GetClienteByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetClienteByIdQuery, ClienteResponse>
{
    public async Task<Result<ClienteResponse>> Handle(GetClienteByIdQuery query, CancellationToken cancellationToken)
    {
        var response = await context.Clientes
            .Where(c => c.Id == query.ClienteId)
            .Select(c => new ClienteResponse(
                c.Id,
                c.Nome,
                c.Email != null ? c.Email.Value : "", // Projection from VO
                c.Telefone,
                c.Tipo == TipoPessoa.Fisica ? (c.Cpf != null ? c.Cpf.Value : "") : (c.Cnpj != null ? c.Cnpj.Value : ""),
                c.CodigoCliente))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            return Result.Failure<ClienteResponse>(ClienteErrors.NotFound(query.ClienteId));
        }

        return response;
    }
}

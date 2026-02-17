using UsinaArtico.Domain.Enums;

namespace UsinaArtico.Application.Users.GetList;

public sealed record UserListResponse
{
    public Guid Id { get; init; }

    public string? Email { get; init; } = string.Empty;

    public string Nome { get; init; } = string.Empty;

    public string? NivelAcesso { get; init; } = string.Empty;
    public bool IsActive { get; set; }
    
}
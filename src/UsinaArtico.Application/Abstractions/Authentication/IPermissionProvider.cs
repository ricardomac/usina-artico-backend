namespace UsinaArtico.Application.Abstractions.Authentication;

public interface IPermissionProvider
{
    Task<HashSet<string>> GetForUserIdAsync(Guid userId);
}

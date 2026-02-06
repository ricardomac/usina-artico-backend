namespace UsinaArtico.Application.Users.GetById;

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public List<string> Roles { get; init; } = [];

    public List<string> Permissions { get; init; } = [];
}

namespace UsinaArtico.Application.Users.GetById;

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }= null!;

    public string FirstName { get; init; }= null!;

    public string LastName { get; init; }= null!;

    public string RoleName { get; init; } = null!;

    public List<string> Permissions { get; init; } = [];

}

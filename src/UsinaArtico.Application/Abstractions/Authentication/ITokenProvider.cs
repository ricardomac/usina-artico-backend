using UsinaArtico.Domain.Users;

namespace UsinaArtico.Application.Abstractions.Authentication;


public interface ITokenProvider
{
    string Create(User user);
}

using CleanSSO.Domain.Entities;

namespace CleanSSO.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}

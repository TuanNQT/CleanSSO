using CleanSSO.Domain.Entities;

namespace CleanSSO.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByProviderAsync(string provider, string providerId);
    Task AddAsync(User user);
    Task AddProviderAsync(UserProvider provider);
    Task SaveChangesAsync();
}

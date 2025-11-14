using CleanSSO.Application.Interfaces;
using CleanSSO.Domain.Entities;
using CleanSSO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanSSO.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task AddProviderAsync(UserProvider provider)
    {
        await _db.UserProviders.AddAsync(provider);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users.Include(u => u.Providers).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByProviderAsync(string provider, string providerId)
    {
        var up = await _db.UserProviders.Include(u => u.User)
            .FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderId == providerId);
        return up?.User;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}

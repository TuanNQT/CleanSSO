using CleanSSO.Application.DTOs;
using CleanSSO.Application.Interfaces;
using CleanSSO.Domain.Entities;

namespace CleanSSO.Application.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository repo, ITokenService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> HandleExternalLoginAsync(string provider, string providerId, string email, string? name, string? picture)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(providerId))
            throw new ArgumentException("Missing login data");

        var user = await _repo.GetByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                Email = email,
                DisplayName = name,
                PictureUrl = picture
            };
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
        }

        var existing = await _repo.GetByProviderAsync(provider, providerId);
        if (existing == null)
        {
            var up = new UserProvider
            {
                UserId = user.Id,
                Provider = provider,
                ProviderId = providerId,
                RawJson = string.Empty
            };
            await _repo.AddProviderAsync(up);
            await _repo.SaveChangesAsync();
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse(token, user.Id, user.Email, user.DisplayName, user.PictureUrl);
    }
}

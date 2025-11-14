namespace CleanSSO.Application.DTOs;

public record AuthResponse(string Token, Guid UserId, string Email, string? DisplayName, string? PictureUrl);

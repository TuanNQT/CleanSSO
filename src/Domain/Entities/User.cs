using System.ComponentModel.DataAnnotations;

namespace CleanSSO.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Email { get; set; } = null!;

    public string? DisplayName { get; set; }
    public string? PictureUrl { get; set; }

    public List<UserProvider> Providers { get; set; } = new();
}

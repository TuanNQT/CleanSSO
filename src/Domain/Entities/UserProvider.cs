using System.ComponentModel.DataAnnotations;

namespace CleanSSO.Domain.Entities;

public class UserProvider
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    public User? User { get; set; }

    [Required]
    public string Provider { get; set; } = null!;

    [Required]
    public string ProviderId { get; set; } = null!;

    public string? RawJson { get; set; }
}

using CleanSSO.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanSSO.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProvider> UserProviders => Set<UserProvider>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<UserProvider>(b =>
        {
            b.HasKey(up => up.Id);
            b.HasIndex(up => new { up.Provider, up.ProviderId }).IsUnique();

            b.HasOne(up => up.User)
                .WithMany(u => u.Providers)
                .HasForeignKey(up => up.UserId);
        });
    }
}

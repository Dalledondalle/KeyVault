using KeyVault.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KeyVault.Data;

public class KeyVaultContext : IdentityDbContext<KeyVaultUser>
{
    public KeyVaultContext(DbContextOptions<KeyVaultContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<KeyVaultUser>().HasMany(x => x.KeyVaultKeys).WithOne(x => x.Owner);

        builder.Entity<KeyVaultKeyUser>().HasKey(x => new {x.KeyVaultUserId, x.KeyVaultKeyId});

        builder.Entity<KeyVaultKeyUser>().HasOne(x => x.KeyVaultUser).WithMany(x => x.AccessibleKeys).HasForeignKey(x => x.KeyVaultUserId);

        builder.Entity<KeyVaultKeyUser>().HasOne(x => x.KeyVaultKey).WithMany(x => x.AccesiblesUsers).HasForeignKey(x => x.KeyVaultKeyId);


        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new KeyVaultUserEntityConfiguration());
    }

    public DbSet<KeyVaultKey> KeyVaultKeys { get; set; }
    public DbSet<KeyVaultUser> KeyVaultUsers { get; set; }
    public DbSet<KeyVaultKeyUser> KeyVaultKeyUser { get; set; }
}

public class KeyVaultUserEntityConfiguration : IEntityTypeConfiguration<KeyVaultUser>
{
    public void Configure(EntityTypeBuilder<KeyVaultUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}
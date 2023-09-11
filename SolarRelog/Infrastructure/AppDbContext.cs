using Microsoft.EntityFrameworkCore;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt){}
    
    public DbSet<SettingsEntity> Settings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var entity = modelBuilder.Entity<SettingsEntity>();
        
        entity.ToTable("settings");
        entity.HasKey(x => x.Id);
        entity.OwnsOne(x => x.AppLogSettings)
            .Property(x => x.MinLogLevel)
            .HasConversion<string>();
    }
}
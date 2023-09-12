using Microsoft.EntityFrameworkCore;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt){}
    
    public DbSet<SettingsEntity> Settings { get; set; } = null!;

    public DbSet<DeviceEntity> Devices { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var settingsEntity = modelBuilder.Entity<SettingsEntity>();
        
        settingsEntity.ToTable("settings");
        settingsEntity.HasKey(x => x.Id);
        settingsEntity.OwnsOne(x => x.AppLogSettings)
            .Property(x => x.MinLogLevel)
            .HasConversion<string>();

        var deviceEntity = modelBuilder.Entity<DeviceEntity>();

        deviceEntity.ToTable("devices");
        deviceEntity.HasKey(x => x.Id);
    }
}
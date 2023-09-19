using Microsoft.EntityFrameworkCore;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt){}
    
    public DbSet<SettingsEntity> Settings { get; set; } = null!;

    public DbSet<DeviceEntity> Devices { get; set; } = null!;

    public DbSet<LogDataEntity> Logs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var settingsEntity = modelBuilder.Entity<SettingsEntity>();
        settingsEntity.ToTable("settings");
        settingsEntity.HasKey(x => x.Id);
        settingsEntity.OwnsOne(x => x.AppLogSettings)
            .Property(x => x.MinLogLevel)
            .HasConversion<string>();
        settingsEntity.OwnsOne(x => x.DataLogSettings);
        settingsEntity.OwnsOne(x => x.InfluxSettings);

        var deviceEntity = modelBuilder.Entity<DeviceEntity>();
        deviceEntity.ToTable("devices");
        deviceEntity.HasKey(x => x.Id);

        var logEntity = modelBuilder.Entity<LogDataEntity>();
        logEntity.ToTable("log_data");
        logEntity.HasKey(x => x.Id);
        logEntity.HasOne(x => x.Device)
            .WithMany(x => x.LogData)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        var consumerLogEntity = modelBuilder.Entity<LogConsumerData>();
        consumerLogEntity.ToTable("log_consumer_data");
        consumerLogEntity.HasKey(x => x.Id);
        consumerLogEntity.HasOne(x => x.LogData)
            .WithMany(x => x.ConsumerData)
            .HasForeignKey(x => x.LogDataId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
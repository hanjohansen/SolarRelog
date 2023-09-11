using Microsoft.EntityFrameworkCore;

namespace SolarRelog.Application.Logging.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions opt) : base(opt){}
    
    public DbSet<LogEntity> Logs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var entity = modelBuilder.Entity<LogEntity>();
        
        entity.ToTable("logs");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Level)
            .HasConversion<string>();
    }
}
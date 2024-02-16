using Microsoft.EntityFrameworkCore;
using SolarPowerPlant.PowerPlants;
using SolarPowerPlant.Users;

namespace SolarPowerPlant.Data;

public class PowerPlantContext : DbContext
{
    private readonly DbUserTrackingService _dbUserTrackingService;

    public PowerPlantContext(
        DbContextOptions<PowerPlantContext> options,
        DbUserTrackingService dbUserTrackingService
    )
        : base(options)
    {
        _dbUserTrackingService = dbUserTrackingService;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<PowerPlant> PowerPlants { get; set; }
    public DbSet<ProductionData> ProductionData { get; set; }

    private void seedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(User.SYSTEM_USER);

        DataGenerator.GeneratePowerPlantData(modelBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductionData>().HasIndex(e => new { e.Type, e.Timestamp });

        seedData(modelBuilder);
    }

    public override int SaveChanges()
    {
        SoftDelete.ProcessSoftDeletedItems(ChangeTracker);

        if (_dbUserTrackingService != null)
        {
            UserChangeTracker.ProcessUserChangeTrackedItems(
                ChangeTracker,
                _dbUserTrackingService.GetCurrentUserId(User.SYSTEM_USER.Id)
            );
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        SoftDelete.ProcessSoftDeletedItems(ChangeTracker);

        if (_dbUserTrackingService != null)
        {
            UserChangeTracker.ProcessUserChangeTrackedItems(
                ChangeTracker,
                _dbUserTrackingService.GetCurrentUserId(User.SYSTEM_USER.Id)
            );
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

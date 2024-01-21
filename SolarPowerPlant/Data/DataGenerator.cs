using Microsoft.EntityFrameworkCore;
using SolarPowerPlant.PowerPlants;

namespace SolarPowerPlant.Data;

public static class DataGenerator
{
    public static void GeneratePowerPlantData(ModelBuilder modelBuilder)
    {
        for (int i = 1; i <= 5; i++)
        {
            var baseDate = DateTime.UtcNow.AddYears(-i);
            var solarPowerPlant = new PowerPlant
            {
                Name = $"Solar Plant {i}",
                InstalledPower = i * 1000,
                DateOfInstallation = baseDate,
                LocationLatitude = GetRandomLatitude(),
                LocationLongitude = GetRandomLongitude(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<PowerPlant>().HasData(solarPowerPlant);

            var productionDate = baseDate;

            for (int j = 0; j < 400; j++)
            {
                productionDate = productionDate.AddMinutes(15);
                modelBuilder
                    .Entity<ProductionData>()
                    .HasData(
                        new ProductionData
                        {
                            PowerPlantId = solarPowerPlant.Id,
                            Timestamp = productionDate,
                            ProductionValue = GetRandomProductionValue(),
                            Type = ProductionType.Actual
                        }
                    );
            }

            productionDate = baseDate;

            for (int j = 0; j < 400; j++)
            {
                productionDate = productionDate.AddMinutes(15);
                modelBuilder
                    .Entity<ProductionData>()
                    .HasData(
                        new ProductionData
                        {
                            PowerPlantId = solarPowerPlant.Id,
                            Timestamp = productionDate,
                            ProductionValue = GetRandomProductionValue(),
                            Type = ProductionType.Forecasted
                        }
                    );
            }
        }
    }

    private static double GetRandomLatitude()
    {
        Random rand = new Random();
        double latitude = rand.NextDouble() * 180 - 90;
        return Math.Round(latitude, 6);
    }

    private static double GetRandomLongitude()
    {
        Random rand = new Random();
        double longitude = rand.NextDouble() * 360 - 180;
        return Math.Round(longitude, 6);
    }

    private static double GetRandomProductionValue()
    {
        return new Random().NextDouble() * 1000;
    }
}

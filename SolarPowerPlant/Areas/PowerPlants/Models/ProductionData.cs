using SolarPowerPlant.Data;
using SolarPowerPlant.Helpers;

namespace SolarPowerPlant.PowerPlants;

public class ProductionData
{
    public Guid Id { get; set; } = IdProvider.NewId();
    public Guid PowerPlantId { get; set; }
    public PowerPlant PowerPlant { get; set; }
    public ProductionType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public double ProductionValue { get; set; }
}

public enum ProductionType
{
    Actual,
    Forecasted
}

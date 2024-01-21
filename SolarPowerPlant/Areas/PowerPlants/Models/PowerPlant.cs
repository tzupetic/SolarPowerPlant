using SolarPowerPlant.Data;
using SolarPowerPlant.Helpers;

namespace SolarPowerPlant.PowerPlants
{
    public class PowerPlant : UserChangeTracked, IIsDeleted
    {
        public Guid Id { get; set; } = IdProvider.NewId();
        public string Name { get; set; }
        public double InstalledPower { get; set; }
        public DateTime DateOfInstallation { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public bool IsDeleted { get; set; }

        public List<ProductionData> ProductionData { get; set; }
    }
}

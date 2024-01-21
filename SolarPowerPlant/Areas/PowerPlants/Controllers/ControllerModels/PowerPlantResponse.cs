using System.ComponentModel.DataAnnotations;

namespace SolarPowerPlant.PowerPlants
{
    public class PowerPlantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double InstalledPower { get; set; }
        public DateTime DateOfInstallation { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SolarPowerPlant.PowerPlants
{
    public class PowerPlantRequest
    {
        public string Name { get; set; }

        [Required]
        public double InstalledPower { get; set; }

        [Required]
        public DateTime DateOfInstallation { get; set; }

        [Required]
        public double LocationLatitude { get; set; }

        [Required]
        public double LocationLongitude { get; set; }
    }
}

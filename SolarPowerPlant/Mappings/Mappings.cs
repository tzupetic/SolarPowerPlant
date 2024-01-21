using AutoMapper;
using SolarPowerPlant.PowerPlants;

namespace SolarPowerPlant.Mappings;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<PowerPlantRequest, PowerPlant>();
        CreateMap<PowerPlant, PowerPlantResponse>();
    }
}

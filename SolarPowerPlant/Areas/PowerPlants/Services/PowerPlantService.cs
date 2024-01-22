using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SolarPowerPlant.Data;
using SolarPowerPlant.Helpers;

namespace SolarPowerPlant.PowerPlants;

public class PowerPlantService
{
    private readonly PowerPlantContext _context;
    private readonly IMapper _mapper;

    public PowerPlantService(PowerPlantContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PowerPlant> GetPowerPlantById(Guid id)
    {
        return await _context.PowerPlants.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new BadRequestException("Power plant does not exist");
    }

    public async Task<PowerPlantResponse> GetPowerPlant(Guid id)
    {
        var powerPlant = await GetPowerPlantById(id);
        var response = _mapper.Map<PowerPlantResponse>(powerPlant);
        return response;
    }

    public async Task<PowerPlantResponse> AddPowerPlant(PowerPlantRequest request)
    {
        var powerPlant = _mapper.Map<PowerPlant>(request);
        _context.Add(powerPlant);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<PowerPlantResponse>(powerPlant);
        return response;
    }

    public async Task<PowerPlantResponse> UpdatePowerPlant(Guid id, PowerPlantRequest request)
    {
        var powerPlant = await GetPowerPlantById(id);
        _mapper.Map(request, powerPlant);
        _context.Update(powerPlant);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<PowerPlantResponse>(powerPlant);
        return response;
    }

    public async Task DeletePowerPlant(Guid id)
    {
        var powerPlant = await GetPowerPlantById(id);
        _context.Remove(powerPlant);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<PowerPlantResponse>> GetPowerPlants(
        int page,
        int pageSize,
        string searchValue
    )
    {
        var response = await (
            from p in _context.PowerPlants
            where !p.IsDeleted && p.Name.ToLower().Contains(searchValue.ToLower())
            orderby p.Name
            select new PowerPlantResponse
            {
                Id = p.Id,
                Name = p.Name,
                InstalledPower = p.InstalledPower,
                DateOfInstallation = p.DateOfInstallation,
                LocationLatitude = p.LocationLatitude,
                LocationLongitude = p.LocationLongitude
            }
        ).GetPagedAsync(page, pageSize);

        return response;
    }

    public async Task<PagedResult<ProductionDataResponse>> GetTimeSeries(
        Guid id,
        TimeSeriesRequest request,
        int page = 1,
        int pageSize = 10
    )
    {
        if (
            !Enum.IsDefined(typeof(ProductionType), request.TimeseriesType)
            || (
                request.Granularity != Granularity.FifteenMinutes
                && request.Granularity != Granularity.OneHour
            )
        )
        {
            throw new BadRequestException("Invalid request parameters.");
        }

        var powerPlant = await _context.PowerPlants.FirstOrDefaultAsync(
            p => p.Id == id && !p.IsDeleted
        );

        if (powerPlant == null)
        {
            throw new BadRequestException("Solar power plant not found.");
        }

        var granularity = request.Granularity == Granularity.OneHour ? 60 : 15;
        var startTime = request.StartTime.HasValue
            ? DateTime.SpecifyKind(request.StartTime.Value, DateTimeKind.Utc)
            : (DateTime?)null;
        var endTime = request.EndTime.HasValue
            ? DateTime.SpecifyKind(request.EndTime.Value, DateTimeKind.Utc)
            : (DateTime?)null;

        var productionData = await _context.ProductionData
            .Where(
                d =>
                    d.PowerPlantId == id
                    && d.Type == request.TimeseriesType
                    && (!startTime.HasValue || d.Timestamp >= startTime)
                    && (!endTime.HasValue || d.Timestamp <= endTime)
            )
            .GroupBy(
                d =>
                    new
                    {
                        d.Timestamp.Year,
                        d.Timestamp.Month,
                        d.Timestamp.Day,
                        d.Timestamp.Hour,
                        Minute = (d.Timestamp.Minute / granularity) * granularity,
                        d.Timestamp.Second
                    }
            )
            .Select(
                g =>
                    new ProductionDataResponse
                    {
                        Timestamp = new DateTime(
                            g.Key.Year,
                            g.Key.Month,
                            g.Key.Day,
                            g.Key.Hour,
                            g.Key.Minute,
                            g.Key.Second
                        ),
                        ProductionValue = g.Sum(d => d.ProductionValue),
                        Type = g.First().Type == ProductionType.Actual ? "Actual" : "Forecasted",
                    }
            )
            .OrderBy(d => d.Timestamp)
            .GetPagedAsync(page, pageSize);

        return productionData;
    }
}

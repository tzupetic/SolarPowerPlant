using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SolarPowerPlant.Helpers;

namespace SolarPowerPlant.PowerPlants;

[ApiController]
[Route("/api/power-plants")]
[Authorize]
[ApiVersion("1.0")]
[EnableRateLimiting("fixed")]
public class PowerPlantController : ControllerBaseExtended
{
    private readonly PowerPlantService _powerPlantService;
    private readonly ChartService _chartService;

    public PowerPlantController(PowerPlantService powerPlantService, ChartService chartService)
    {
        _powerPlantService = powerPlantService;
        _chartService = chartService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PowerPlantResponse>> GetPowerPlant(Guid id)
    {
        return Ok(await _powerPlantService.GetPowerPlant(id));
    }

    [HttpPost()]
    public async Task<ActionResult<PowerPlantResponse>> AddPowerPlant(
        [FromBody] PowerPlantRequest request
    )
    {
        return Ok(await _powerPlantService.AddPowerPlant(request));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PowerPlantResponse>> UpdatePowerPlant(
        [FromBody] PowerPlantRequest request,
        Guid id
    )
    {
        return Ok(await _powerPlantService.UpdatePowerPlant(id, request));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePowerPlant(Guid id)
    {
        await _powerPlantService.DeletePowerPlant(id);
        return Ok();
    }

    [HttpGet()]
    public async Task<ActionResult<PagedResult<PowerPlantResponse>>> GetPowerPlants(
        int page = 1,
        int pageSize = 10,
        string name = ""
    )
    {
        var response = await _powerPlantService.GetPowerPlants(page, pageSize, name);

        return Ok(response);
    }

    [HttpGet("{id}/time-series")]
    public async Task<ActionResult<PagedResult<PowerPlantResponse>>> GetTimeSeries(
        Guid id,
        [FromQuery] TimeSeriesRequest request,
        int page = 1,
        int pageSize = 10
    )
    {
        var response = await _powerPlantService.GetTimeSeries(id, request, page, pageSize);

        return Ok(response);
    }

    [HttpGet("{id}/generate-chart")]
    public async Task<ActionResult> GenerateChart(Guid id, [FromQuery] DateOnly date)
    {
        await _chartService.GenerateChart(id, date);
        return Ok();
    }
}

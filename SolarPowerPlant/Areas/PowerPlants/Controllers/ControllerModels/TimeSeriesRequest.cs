namespace SolarPowerPlant.PowerPlants;

public class TimeSeriesRequest
{
    public ProductionType TimeseriesType { get; set; }
    public Granularity Granularity { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

public enum Granularity
{
    FifteenMinutes,
    OneHour
}

namespace SolarPowerPlant.Helpers;

public class StaticConfiguration
{
    private static IConfiguration _configuration;

    public static string ConnectionStringsDB =>
        _configuration.GetValue<string>("ConnectionStrings:PowerPlantDB");

    public static string PythonConnectionStringDB =>
        _configuration.GetValue<string>("ConnectionStrings:PythonConnectionString");

    public static string AppSettingsSecret => _configuration.GetValue<string>("AppSettings:Secret");

    public static string AppSettingsExpirationDays =>
        _configuration.GetValue<string>("AppSettings:ExpirationDays");

    public static int PermitLimit => _configuration.GetValue<int>("RateLimits:PermitLimit");
    public static int RateLimitSeconds => _configuration.GetValue<int>("RateLimits:Seconds");

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}

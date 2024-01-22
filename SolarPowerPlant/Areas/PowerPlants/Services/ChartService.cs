using Microsoft.EntityFrameworkCore;
using SolarPowerPlant.Data;
using SolarPowerPlant.Helpers;
using System.Diagnostics;

namespace SolarPowerPlant.PowerPlants;

public class ChartService
{
    private readonly PowerPlantService _powerPlantService;

    public ChartService(PowerPlantService powerPlantService)
    {
        _powerPlantService = powerPlantService;
    }

    public async Task GenerateChart(Guid powerPlantId, DateOnly date)
    {
        await _powerPlantService.GetPowerPlantById(powerPlantId);

        var dbConnectionString = StaticConfiguration.PythonConnectionStringDB;
        var dateValue = date.ToString("yyyy-MM-dd");
        var filePath = $"/app/charts/line_chart_{DateTime.UtcNow.Ticks}.png";
        var command =
            $"exec chart_generator python /app/charts/generate_chart.py \"{dbConnectionString}\" \"{powerPlantId}\" \"{dateValue}\" \"{filePath}\"";

        await RunPythonScript(command);
    }

    private async Task RunPythonScript(string command)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = command,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = startInfo, })
        {
            process.Start();
            process.WaitForExit();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception(
                    $"Error while executing python script. Exit code: {process.ExitCode}. Error output: {error}"
                );
            }

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Warning or non-error message: {error}");
            }

            Console.WriteLine($"Python script output: {output}");
        }
    }
}

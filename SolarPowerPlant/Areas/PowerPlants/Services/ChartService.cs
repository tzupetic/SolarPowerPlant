using Microsoft.EntityFrameworkCore;
using SolarPowerPlant.Data;
using SolarPowerPlant.Helpers;
using System.Diagnostics;

namespace SolarPowerPlant.PowerPlants;

public class ChartService
{
    private readonly PowerPlantContext _context;

    public ChartService(PowerPlantContext context)
    {
        _context = context;
    }

    public async Task GenerateChart(Guid powerPlantId, DateOnly date)
    {
        var powerPlant = await _context.PowerPlants.FirstOrDefaultAsync(
            p => p.Id == powerPlantId && !p.IsDeleted
        );

        if (powerPlant == null)
        {
            throw new BadRequestException("Power plant does not exist");
        }

        var dbConnectionString = StaticConfiguration.PythonConnectionStringDB;
        var dateValue = date.ToString("yyyy-MM-dd");
        var fileName = $"line_chart_{DateTime.UtcNow.Ticks}.png";
        var command =
            $"exec chart_generator python generate_chart.py \"{dbConnectionString}\" \"{powerPlantId}\" \"{dateValue}\" \"{fileName}\"";

        await RunPythonScript(command);

        var containerPath = $"/app/{fileName}";
        var localPath = Path.Combine("LineCharts", fileName);
        command = $"cp chart_generator:{containerPath} {localPath}";

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

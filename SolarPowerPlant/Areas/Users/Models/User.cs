using SolarPowerPlant.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SolarPowerPlant.Users;

public class User
{
    public static readonly User SYSTEM_USER = new User
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Email = "admin@powerplant.com",
        Password = "",
        FirstName = "System",
        LastName = "",
        CreatedAt = new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc)
    };
    public Guid Id { get; set; } = IdProvider.NewId();

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

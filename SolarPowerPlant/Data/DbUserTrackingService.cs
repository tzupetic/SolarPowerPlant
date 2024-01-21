using SolarPowerPlant.Helpers;
using SolarPowerPlant.Users;
using System.Security.Claims;

namespace SolarPowerPlant.Data;

public class DbUserTrackingService
{
    private readonly IHttpContextAccessor _httpContext;

    public DbUserTrackingService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public Guid GetCurrentUserId(Guid? fallbackUserId = null)
    {
        // for members we're always returning the system user id (1)

        if (_httpContext == null || _httpContext.HttpContext == null)
        {
            if (fallbackUserId.HasValue)
                return fallbackUserId.Value;

            throw new UnauthorizedException("Can't find logged in user");
        }

        if (
            _httpContext.HttpContext.User?.Claims == null
            || !(_httpContext.HttpContext.User?.Claims).Any()
        )
            return User.SYSTEM_USER.Id;

        var userIdClaim = _httpContext.HttpContext.User.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.NameIdentifier
        );

        if (userIdClaim?.Value == null)
            return User.SYSTEM_USER.Id;

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedException("Error parsing logged in user");
        }

        return userId;
    }
}

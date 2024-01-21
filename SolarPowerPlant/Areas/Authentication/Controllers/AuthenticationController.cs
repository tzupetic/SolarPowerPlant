using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SolarPowerPlant.Helpers;

namespace SolarPowerPlant.Authentication;

[ApiController]
[Route("/api/authentication")]
[ApiVersion("1.0")]
[EnableRateLimiting("fixed")]
public class AuthenticationController : ControllerBaseExtended
{
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("registration")]
    public async Task<ActionResult<UserAuthenticationResponse>> Register(
        [FromBody] RegistrationRequest model
    )
    {
        return Created("", await _authenticationService.Register(model));
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserAuthenticationResponse>> Authenticate(
        [FromBody] UserLoginRequest model
    )
    {
        return Ok(await _authenticationService.Authenticate(model.Email, model.Password));
    }
}

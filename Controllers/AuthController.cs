using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;

[ApiController]
[Route("api/[controller]")]
[Tags("Login")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Invalid login request." });

        var result = await _authService.LoginAsync(dto);

        if (result == null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(new
        {
            message = "Login successful.",
            token = result.Token,
            expiresAt = result.ExpiresAt,
            user = new
            {
                result.User.UserId,
                result.User.UserName,
                result.User.Email,
                result.User.RoleId
            }
        });
    }

}


using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
public class UserAccountsController : ControllerBase
{
    private readonly IUserService _userService;

    public UserAccountsController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all users.")]
    // Retrieves all users with optional search filter.
    public async Task<ActionResult<List<UserAccountDTO>>> Get([FromQuery] string? search)
    {
        var users = await _userService.GetAllAsync(search);
        return Ok(users);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get user by ID.")]
    // Retrieves a single user by ID.
    public async Task<ActionResult<UserAccountDTO>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(user);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new user.")]
    // Creates a new user and returns the new ID.
    public async Task<ActionResult<int>> Create(UserAccountCreateDTO dto)
    {
        var newId = await _userService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById),
            new { id = newId },
            new { message = "User created successfully.", id = newId });
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update user information.")]
    // Updates general user data.
    public async Task<IActionResult> Update(int id, UserUpdateDTO dto)
    {
        if (id != dto.UserId)
            return BadRequest(new { message = "User ID in URL does not match User ID in body." });

        var updated = await _userService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = "User updated successfully." });
    }

    [HttpPut("ChangeStatus/{id}")]
    [SwaggerOperation(Summary = "Update user status.")]
    // Activates or deactivates a user.
    public async Task<IActionResult> ChangeStatus(int id, UserActiveDTO dto)
    {
        if (id != dto.UserId)
            return BadRequest(new { message = "User ID in URL does not match User ID in body." });

        var updated = await _userService.ChangeStatus(id, dto);

        if (!updated)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = "User status changed successfully." });
    }

    [HttpPut("ChangeRole/{id}")]
    [SwaggerOperation(Summary = "Update user role.")]
    // Updates the user's assigned role.
    public async Task<IActionResult> ChangeRole(int id, UserRoleDTO dto)
    {
        if (id != dto.UserId)
            return BadRequest(new { message = "User ID in URL does not match User ID in body." });

        var updated = await _userService.ChangeRole(id, dto);

        if (!updated)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = "User role changed successfully." });
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete user.")]
    // Deletes a user permanently.
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = $"User with ID {id} deleted successfully." });
    }
}

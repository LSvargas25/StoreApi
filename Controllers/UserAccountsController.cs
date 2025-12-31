using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class UserAccountsController : ControllerBase
{
    private readonly IUserService _userService;

    public UserAccountsController(IUserService userService)
    {
        _userService = userService;
    }

    // -------------------------------------------------------------
    // GET ALL USERS (ACTIVOS / INACTIVOS / TODOS)
    // -------------------------------------------------------------
    [HttpGet]
    [SwaggerOperation(Summary = "Get all users.")]
    public async Task<ActionResult<List<UserAccountDTO>>> Get(
        [FromQuery] string? search,
        [FromQuery] bool? isActive
    )
    {
        var users = await _userService.GetAllAsync(search);

        if (isActive.HasValue)
            users = users.Where(u => u.IsActive == isActive.Value).ToList();

        return Ok(users);
    }

    // -------------------------------------------------------------
    // GET USER BY ID
    // -------------------------------------------------------------
    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get user by ID.")]
    public async Task<ActionResult<UserAccountDTO>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(user);
    }

    // -------------------------------------------------------------
    // CREATE USER
    // -------------------------------------------------------------
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new user.")]
    public async Task<ActionResult> Create([FromBody] UserAccountCreateDTO dto)
    {
        var newId = await _userService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = newId },
            new { message = "User created successfully.", id = newId }
        );
    }

    // -------------------------------------------------------------
    // UPDATE USER (INCLUDES ROLE CHANGE)
    // -------------------------------------------------------------
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update user information.")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Request body is required." });

        var updated = await _userService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = "User updated successfully." });
    }

    // -------------------------------------------------------------
    // CHANGE STATUS
    // -------------------------------------------------------------
    [HttpPut("ChangeStatus/{id:int}")]
    [SwaggerOperation(Summary = "Update user status.")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] UserActiveDTO dto)
    {
        if (id != dto.UserId)
            return BadRequest(new { message = "User ID mismatch." });

        var updated = await _userService.ChangeStatus(id, dto);

        if (!updated)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = "User status changed successfully." });
    }

    // -------------------------------------------------------------
    // DELETE USER
    // -------------------------------------------------------------
    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete user.")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = $"User with ID {id} not found." });

        return Ok(new { message = $"User with ID {id} deleted successfully." });
    }

    [HttpPut("{id}/photo")]
    public async Task<IActionResult> EditPhoto(int id, [FromBody] UserImageDTO dto)
    {
        var result = await _userService.EditPhoto(id, dto);

        if (!result)
            return NotFound(new { message = "User not found." });

        return Ok(new { message = "Photo updated successfully." });
    }

}

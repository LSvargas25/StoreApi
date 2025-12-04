using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using StoreApi.Interface.User;
using StoreApi.ModelsDTO.User;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IUserRoleService _roleService;

        public RolesController(IUserRoleService roleService)
        {
            _roleService = roleService;
        }
            // Get all the UserRole created on the bd 
        [HttpGet]
        [SwaggerOperation(Summary = "Get all the UserRole on the bd ")]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var data = await _roleService.GetAllAsync(search, page, limit);
            return Ok(data);
        }

        //Get Userole by ID
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get all the UserRole by ID ")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }
        //Create a new Userole
        [HttpPost]
        [SwaggerOperation(Summary = "Register a new UserRole")]
        public async Task<IActionResult> Create([FromBody] RoleDTO dto)
        {
            var id = await _roleService.CreateAsync(dto);
 
            dto.RoleId = id;
 
            return Ok(dto);
        }
        //Update a  Userole by ID
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update UserRole")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDTO dto)
        {
            var updated = await _roleService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return Ok(dto);
        }
        //Delete  UserRole
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete UserRole")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _roleService.DeleteAsync(id);
            if (!deleted) return NotFound();

            // Return 200 with a simple message
            return Ok(new { message = "Role deleted successfully", roleId = id });
        }


    }
}

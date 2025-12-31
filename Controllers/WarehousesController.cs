using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.WareHouse;
using StoreApi.ModelsDTO.WareHouse;
using System.Security.Claims;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/warehouses")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWareHouseService _service;

        public WarehousesController(IWareHouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<WareHouseDTO>>> GetAll([FromQuery] string? search)
        {
            var data = await _service.GetAllAsync(search);
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WareHouseDTO>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WareHouseCreateDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { WarehouseId = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] WareHouseUpdateDTO dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] WareHouseStatusDTO dto)
        {
            if (dto.WarehouseId != id) return BadRequest("WarehouseId mismatch.");

            var ok = await _service.ChangeStatusAsync(id, dto.IsActive);
            if (!ok) return NotFound();
            return NoContent();
        }

            [Authorize(Roles = "Admin")]
            [HttpDelete("{id:int}")]
            public async Task<IActionResult> HardDelete(int id)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized(new { message = "Usuario no autenticado." });

                int userId = int.Parse(userIdClaim);

                var result = await _service.HardDeleteAsync(id, userId);

                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(new { message = result.Message });
            }
        
    }
}

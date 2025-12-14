using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.WareHouse;
using StoreApi.ModelsDTO.WareHouse;

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

        // HARD DELETE (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var ok = await _service.HardDeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

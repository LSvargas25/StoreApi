using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Unit;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/unit-relations")]
    public class UnitRelationsController : ControllerBase
    {
        private readonly IUnitRelationService _service;

        public UnitRelationsController(IUnitRelationService service)
        {
            _service = service;
        }

        // ======================================================
        // GET ALL
        // ======================================================
        [HttpGet]
        [SwaggerOperation(Summary = "Get all unit relations with optional search.")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var result = await _service.GetAllUnitRelationsAsync(search);
            return Ok(result);
        }

        // ======================================================
        // CREATE
        // ======================================================
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Unit Relation.")]
        public async Task<IActionResult> Create(UnitRelationCreateDTO dto)
        {
            await _service.CreateUnitRelationAsync(dto);
            return Ok(new { message = "Unit relation created successfully." });
        }

        // ======================================================
        // UPDATE
        // ======================================================
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing Unit Relation.")]
        public async Task<IActionResult> Update(int id, UnitRelationCreateDTO dto)
        {
            var updated = await _service.UpdateUnitRelationAsync(id, dto);

            if (!updated)
                return BadRequest(new { message = "Unit relation was not updated successfully." });

            return Ok(new { message = "Unit relation updated successfully." });
        }

        // ======================================================
        // CHANGE STATUS
        // ======================================================
        [HttpPatch("{id}/status")]
        [SwaggerOperation(Summary = "Change the active status of a Unit Relation.")]
        public async Task<IActionResult> ChangeStatus(int id, UnitRelationStatus dto)
        {
            var result = await _service.ChangeStatus(id, dto);

            if (!result)
                return BadRequest(new { message = "Unit relation status was not updated successfully." });

            return Ok(new { message = "Unit relation status updated successfully." });
        }

        // ======================================================
        // DELETE
        // ======================================================
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Unit Relation.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteUnitRelationAsync(id);

            if (!deleted)
                return BadRequest(new { message = "Unit relation was not deleted successfully." });

            return Ok(new { message = "Unit relation deleted successfully." });
        }
    }
}

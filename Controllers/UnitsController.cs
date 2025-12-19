using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Unit;

namespace StoreApi.Controllers.Unit
{
    [ApiController]
    [Route("api/units")]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _service;

        public UnitsController(IUnitService service)
        {
            _service = service;
        }

        // ======================================================
        // GET ALL UNITS
        // ======================================================
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all units.",
            Description = "Retrieves all units. Optionally filters results by a search term."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var units = await _service.GetAllUnitsAsync(search);
            return Ok(units);
        }

        // ======================================================
        // CREATE UNIT
        // ======================================================
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new unit.",
            Description = "Creates a new unit and returns the generated unit identifier."
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] UnitCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid unit data.");

            var newUnitId = await _service.CreateUnitAsync(dto);

            if (newUnitId <= 0)
            {
                return BadRequest(new
                {
                    message = "Unit was not created successfully."
                });
            }

            return Created(
                $"api/units/{newUnitId}",
                new
                {
                    unitId = newUnitId,
                    message = "Unit created successfully."
                }
            );
        }

        // ======================================================
        // UPDATE UNIT
        // ======================================================
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Update an existing unit.",
            Description = "Updates the name of an existing unit by its identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UnitCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid unit data.");

            var updated = await _service.UpdateUnitAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Unit was not updated successfully."
                });
            }

            return Ok(new
            {
                message = "Unit updated successfully."
            });
        }

        // ======================================================
        // DELETE UNIT
        // ======================================================
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete a unit.",
            Description = "Deletes an existing unit by its identifier."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteUnitAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Unit was not deleted successfully."
                });
            }

            return Ok(new
            {
                message = "Unit deleted successfully."
            });
        }
    }
}

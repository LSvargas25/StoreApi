using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Unit;

namespace StoreApi.Controllers.Unit
{
    [ApiController]
    [Route("api/unit-conversions")]
    public class UnitConversionsController : ControllerBase
    {
        private readonly IUnitConversionService _service;

        public UnitConversionsController(IUnitConversionService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all unit conversions.")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
            => Ok(await _service.GetAllUnitConversionsAsync(search));

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new unit conversion.")]
        public async Task<IActionResult> Create(UnitConversionCreateDTO dto)
        {
            await _service.CreateUnitConversionAsync(dto);
            return Ok(new { message = "Unit conversion created successfully." });
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update an existing unit conversion.")]
        public async Task<IActionResult> Update(int id, UnitConversionCreateDTO dto)
        {
            var updated = await _service.UpdateUnitConversionAsync(id, dto);

            if (!updated)
                return NotFound(new { message = "Unit conversion was not updated successfully." });

            return Ok(new { message = "Unit conversion updated successfully." });
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete a unit conversion.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteUnitConversionAsync(id);

            if (!deleted)
                return NotFound(new { message = "Unit conversion was not deleted successfully." });

            return Ok(new { message = "Unit conversion deleted successfully." });
        }

        // ===============================
        // CONVERT
        // ===============================
        [HttpPost("convert")]
        [SwaggerOperation(Summary = "Convert a value from one unit to another.")]
        public async Task<IActionResult> Convert(UnitConversionRequestDTO dto)
        {
            var conversion = await _service.ConvertAsync(
                dto.FromUnitId,
                dto.ToUnitId,
                dto.Value);

            return Ok(new
            {
                message = $"{conversion.OriginalValue} {conversion.FromUnitName} = {conversion.ResultValue} {conversion.ToUnitName}",
                result = conversion.ResultValue
            });
        }
    }
}

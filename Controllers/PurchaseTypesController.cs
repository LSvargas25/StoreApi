using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Purchase;
using StoreApi.ModelsDTO.Purshase;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseTypesController : ControllerBase
    {
        private readonly IPurchaseTypeService _purchaseTypeService;

        public PurchaseTypesController(IPurchaseTypeService purchaseTypeService)
        {
            _purchaseTypeService = purchaseTypeService;
        }

        // GET ALL Purchase Types
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Purchase Types.")]
        public async Task<ActionResult<List<PurchaseTypeDTO>>> Get(
            [FromQuery] string? search, int page = 1, int limit = 10)
        {
            var types = await _purchaseTypeService.GetAllAsync(search, page, limit);
            return Ok(types);
        }

        // CREATE Purchase Type
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Purchase Type.")]
        public async Task<IActionResult> Create([FromBody] PurchaseTypeCreate dto)
        {
            var id = await _purchaseTypeService.CreateAsync(dto);

            var response = new PurchaseTypeDTO
            {
                PurchaseTypeId = id,
                Name = dto.Name
            };

            return Ok(response);
        }

        // DELETE Purchase Type
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Purchase Type.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _purchaseTypeService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Purchase Type not found." });

            return Ok(new
            {
                message = "Purchase Type deleted successfully.",
                id
            });
        }

        // UPDATE Purchase Type
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a Purchase Type.")]
        public async Task<IActionResult> Update(int id, [FromBody] PurchaseTypeUpdate dto)
        {
            if (id != dto.PurchaseTypeId)
            {
                return BadRequest(new
                {
                    message = "Purchase Type ID in URL does not match the body."
                });
            }

            var updated = await _purchaseTypeService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Purchase Type with ID {id} not found." });

            return Ok(new { message = "Purchase Type updated successfully." });
        }


        // CHANGE STATUS (IsActive)
        [HttpPut("{id}/status")]
        [SwaggerOperation(Summary = "Change active status of a Purchase Type.")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] PurchaseTypeStatus dto)
        {
            if (id != dto.PurchaseTypeId)
            {
                return BadRequest(new
                {
                    message = "Purchase Type ID in URL does not match the body."
                });
            }

            var result = await _purchaseTypeService.ChangeStatusAsync(id, dto);

            if (!result)
                return NotFound(new { message = $"Purchase Type with ID {id} not found." });

            return Ok(new { message = "Status updated successfully." });
        }
    }
}

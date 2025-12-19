using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StoreApi.Interface.ItemVariant;
using StoreApi.ModelsDTO.Common;
using StoreApi.ModelsDTO.ItemVariant;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/item-variants")]
    public class ItemVariantsController : ControllerBase
    {
        private readonly IItemVariantService _service;

        public ItemVariantsController(IItemVariantService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new item variant.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ItemVariantCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Validation failed.", ModelState));

            var (success, message, newId) = await _service.CreateAsync(dto);

            if (!success)
            {
                if (message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
                    return Conflict(ApiResponse<object>.Fail(message));

                if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(ApiResponse<object>.Fail(message));

                return BadRequest(ApiResponse<object>.Fail(message));
            }

            return Ok(ApiResponse<object>.Ok(message, new { ItemVariantId = newId }));
        }

        [HttpGet("{itemVariantId:int}")]
        [SwaggerOperation(Summary = "Get an item variant by ID.")]
        [ProducesResponseType(typeof(ApiResponse<ItemVariantDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ItemVariantDTO>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int itemVariantId)
        {
            var (success, message, data) = await _service.GetByIdAsync(itemVariantId);
            if (!success) return NotFound(ApiResponse<ItemVariantDTO>.Fail(message));
            return Ok(ApiResponse<ItemVariantDTO>.Ok(message, data));
        }

        [HttpPut("{itemVariantId:int}")]
        [SwaggerOperation(Summary = "Update an item variant.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update([FromRoute] int itemVariantId, [FromBody] ItemVariantUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Validation failed.", ModelState));

            var (success, message) = await _service.UpdateAsync(itemVariantId, dto);

            if (!success)
            {
                if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(ApiResponse<object>.Fail(message));

                if (message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
                    return Conflict(ApiResponse<object>.Fail(message));

                return BadRequest(ApiResponse<object>.Fail(message));
            }

            return Ok(ApiResponse<object>.Ok(message));
        }

        [HttpDelete("{itemVariantId:int}")]
        [SwaggerOperation(Summary = "Delete an item variant.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int itemVariantId)
        {
            var (success, message) = await _service.DeleteAsync(itemVariantId);
            if (!success) return NotFound(ApiResponse<object>.Fail(message));
            return Ok(ApiResponse<object>.Ok(message));
        }

        [HttpPatch("{itemVariantId:int}/status")]
        [SwaggerOperation(Summary = "Change item variant active status.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus([FromRoute] int itemVariantId, [FromBody] ItemVariantStatus dto)
        {
            var (success, message) = await _service.ChangeStatusAsync(itemVariantId, dto.IsActive);
            if (!success) return NotFound(ApiResponse<object>.Fail(message));
            return Ok(ApiResponse<object>.Ok(message));
        }

        [HttpGet("active")]
        [SwaggerOperation(Summary = "List all active item variants.")]
        [ProducesResponseType(typeof(ApiResponse<List<ListVariantDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive([FromQuery] string? search = null)
        {
            var data = await _service.GetAllActiveAsync(search);
            return Ok(ApiResponse<List<ListVariantDTO>>.Ok("Active item variants retrieved successfully.", data));
        }

        [HttpGet("inactive")]
        [SwaggerOperation(Summary = "List all inactive item variants.")]
        [ProducesResponseType(typeof(ApiResponse<List<ListVariantDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInactive([FromQuery] string? search = null)
        {
            var data = await _service.GetAllInactiveAsync(search);
            return Ok(ApiResponse<List<ListVariantDTO>>.Ok("Inactive item variants retrieved successfully.", data));
        }

        [HttpGet("item/{itemId:int}/complete")]
        [SwaggerOperation(Summary = "List complete item variants by item ID (including unit info).")]
        [ProducesResponseType(typeof(ApiResponse<List<ListCompleteVariantDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComplete([FromRoute] int itemId)
        {
            var data = await _service.GetCompleteListAsync(itemId);
            return Ok(ApiResponse<List<ListCompleteVariantDTO>>.Ok("Complete item variants retrieved successfully.", data));
        }

        [HttpGet("item/{itemId:int}/price-cost")]
        [SwaggerOperation(Summary = "List item variant price and cost by item ID.")]
        [ProducesResponseType(typeof(ApiResponse<List<ListPriceCost>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPriceCost([FromRoute] int itemId)
        {
            var data = await _service.GetPriceCostListAsync(itemId);
            return Ok(ApiResponse<List<ListPriceCost>>.Ok("Item variant price and cost list retrieved successfully.", data));
        }

        [HttpGet("item/{itemId:int}")]
        [SwaggerOperation(Summary = "List item variants by item ID.")]
        [ProducesResponseType(typeof(ApiResponse<List<ListItemVariantByItemIdDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByItemId([FromRoute] int itemId)
        {
            var data = await _service.GetByItemIdAsync(itemId);
            return Ok(ApiResponse<List<ListItemVariantByItemIdDTO>>.Ok("Item variants retrieved successfully.", data));
        }

        // ===== NUEVO: Update Price (writes PriceHistory + updates ItemVariant.StandardPrice) =====
        [HttpPatch("{itemVariantId:int}/price")]
        [SwaggerOperation(Summary = "Update item variant price and create a price history record.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrice([FromRoute] int itemVariantId, [FromBody] UpdateVariantPriceDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Validation failed.", ModelState));

            var (success, message) = await _service.UpdatePriceAsync(itemVariantId, dto);
            if (!success)
            {
                if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(ApiResponse<object>.Fail(message));

                return BadRequest(ApiResponse<object>.Fail(message));
            }

            return Ok(ApiResponse<object>.Ok(message));
        }

        // ===== NUEVO: Update Cost (writes ItemCostHistory + updates ItemVariant.StandardCost) =====
        [HttpPatch("{itemVariantId:int}/cost")]
        [SwaggerOperation(Summary = "Update item variant cost and create a cost history record.")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCost([FromRoute] int itemVariantId, [FromBody] UpdateVariantCostDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Validation failed.", ModelState));

            var (success, message) = await _service.UpdateCostAsync(itemVariantId, dto);
            if (!success)
            {
                if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(ApiResponse<object>.Fail(message));

                return BadRequest(ApiResponse<object>.Fail(message));
            }

            return Ok(ApiResponse<object>.Ok(message));
        }

        // ===== NUEVO: Histories =====
        [HttpGet("{itemVariantId:int}/price-history")]
        [SwaggerOperation(Summary = "Get price history for an item variant.")]
        [ProducesResponseType(typeof(ApiResponse<List<PriceHistoryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPriceHistory([FromRoute] int itemVariantId)
        {
            var (success, message, data) = await _service.GetPriceHistoryAsync(itemVariantId);
            if (!success) return NotFound(ApiResponse<object>.Fail(message));
            return Ok(ApiResponse<List<PriceHistoryDTO>>.Ok(message, data!));
        }

        [HttpGet("{itemVariantId:int}/cost-history")]
        [SwaggerOperation(Summary = "Get cost history for an item variant.")]
        [ProducesResponseType(typeof(ApiResponse<List<ItemCostHistoryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCostHistory([FromRoute] int itemVariantId)
        {
            var (success, message, data) = await _service.GetCostHistoryAsync(itemVariantId);
            if (!success) return NotFound(ApiResponse<object>.Fail(message));
            return Ok(ApiResponse<List<ItemCostHistoryDTO>>.Ok(message, data!));
        }
    }
}

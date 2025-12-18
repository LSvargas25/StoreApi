using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers.Item
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemController(IItemService service)
        {
            _service = service;
        }

        // ======================================================
        // CREATE
        // ======================================================
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new item")]
        public async Task<IActionResult> Create(ItemCreateDTO dto)
        {
            var item = await _service.CreateAsync(dto);

            if (item == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Item not created"
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { itemId = item.ItemId },
                new
                {
                    success = true,
                    message = "Item created successfully",
                    data = item
                });
        }

        // ======================================================
        // UPDATE
        // ======================================================
        [HttpPut("{itemId}")]
        [SwaggerOperation(Summary = "Update an existing item")]
        public async Task<IActionResult> Update(int itemId, ItemUpdateDTO dto)
        {
            var updated = await _service.UpdateAsync(itemId, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Item not found or not updated"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Item updated successfully"
            });
        }

        // ======================================================
        // CHANGE STATUS
        // ======================================================
        [HttpPatch("{itemId}/status")]
        [SwaggerOperation(Summary = "Change item active status")]
        public async Task<IActionResult> ChangeStatus(int itemId, ItemChangeStatus dto)
        {
            var changed = await _service.ChangeStatusAsync(itemId, dto);

            if (!changed)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Item not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Item status updated successfully"
            });
        }




        // ======================================================
        // GET BY ID
        // ======================================================
        [HttpGet("{itemId}")]
        [SwaggerOperation(Summary = "Get item by id")]
        public async Task<IActionResult> GetById(int itemId)
        {
            var item = await _service.GetByIdAsync(itemId);

            if (item == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Item not found"
                });
            }

            return Ok(new
            {
                success = true,
                data = item
            });
        }

        // ======================================================
        // GET ALL
        // ======================================================
        [HttpGet]
        [SwaggerOperation(Summary = "Get Item with image , barcode and name")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var items = await _service.GetAllAsync(search);

            return Ok(new
            {
                success = true,
                data = items
            });
        }

        // ======================================================
        // GET ALL WITH ATTRIBUTES
        // ======================================================
        [HttpGet("with-attributes")]
        [SwaggerOperation(Summary = "Get all items with attributes")]
        public async Task<IActionResult> GetAllWithAttributes([FromQuery] string? search)
        {
            var items = await _service.GetAllWithAttributesAsync(search);

            return Ok(new
            {
                success = true,
                data = items
            });
        }

        // ======================================================
        // DELETE
        // ======================================================
        [HttpDelete("{itemId}")]
        [SwaggerOperation(Summary = "Delete an item permanently")]
        public async Task<IActionResult> Delete(int itemId)
        {
            var deleted = await _service.DeleteAsync(itemId);

            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Item not found or not deleted"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Item deleted successfully"
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using StoreApi.ModelsDTO.Purshase;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCategoriesController : ControllerBase
    {
        private readonly  IItemCategoryService _itemCategoryService;

        public ItemCategoriesController(IItemCategoryService itemCategoryService)
        {
            _itemCategoryService = itemCategoryService;
        }

        //Gell all the Item types
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Item types.")]

        public async Task<ActionResult<List<ItemCategoryDTO>>> Get([FromQuery] string? search, int page = 1, int limit = 10)
        {
            var types = await _itemCategoryService.GetAllAsync(search, page, limit);
            return Ok(types);
        }
        //Create the Customer types
        [HttpPost]
        [SwaggerOperation(Summary = "Create a Item type.")]
        public async Task<IActionResult> Create([FromBody] ItemCatCreate dto)
        {
            var id = await _itemCategoryService.CreateAsync(dto);
            dto.ItemCategoryId = id;
            return Ok(dto);
        }
        //Delete Customer
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Item type.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _itemCategoryService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Item Type type not found" });

            return Ok(new { message = "Item type deleted successfully", id });
        }
        //Update a Item Category 
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a Item Category.")]
        public async Task<IActionResult> UpdateAsync(int id, ItemCatUpdt dto)
        {
            if (id != dto.ItemCategoryId)
                return BadRequest(new { message = "Item Category ID  in URL does not match  in body." });

            var updated = await _itemCategoryService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Item Category with ID {id} not found." });

            return Ok(new { message = "Item Category updated successfully." });
        }


        // CHANGE STATUS (IsActive)
        [HttpPut("{id}/status")]
        [SwaggerOperation(Summary = "Change active status of a Item Category.")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ItemChangStatus dto)
        {
            if (id != dto.ItemCategoryId)
            {
                return BadRequest(new
                {
                    message = "Item Type ID in URL does not match the body."
                });
            }

            var result = await _itemCategoryService.ChangeStatusAsync(id, dto);

            if (!result)
                return NotFound(new { message = $"Category Type with ID {id} not found." });

            return Ok(new { message = "Status updated successfully." });
        }

    }
}

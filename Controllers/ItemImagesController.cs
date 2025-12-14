using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Item")]
    public class ItemImagesController : ControllerBase
    {
        private readonly IItemImageService _itemImageService;

        public ItemImagesController(IItemImageService itemImageService)
        {
            _itemImageService = itemImageService;
        }
        
        //Create a new image for item 
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new image for the item .")]
        public async Task<IActionResult> Create([FromBody] ItemImageDTO dto)
        {
            var id = await _itemImageService.CreateAsync(dto);
            dto.ItemId = id;
            return Ok(dto);
        }
        //Delete Image
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Image.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _itemImageService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Image is deleted or  not found" });

            return Ok(new { message = "Image deleted successfully", id });
        }
        // PUT: api/ItemImages/SetPrimary/10
        [HttpPut("SetPrimary/{imageId}")]
        [SwaggerOperation(Summary = "Set an image as the primary image of its item.")]
        public async Task<IActionResult> SetPrimary(int imageId)
        {
            var updated = await _itemImageService.SetAsPrimaryAsync(imageId);

            if (!updated)
                return NotFound(new { message = "Image not found." });

            return Ok(new { message = "Image set as primary successfully.", imageId });
        }



    }
}

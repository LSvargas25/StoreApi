using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemImagesController : ControllerBase
    {
        private readonly IItemImageService _itemImageService;

        public ItemImagesController(IItemImageService itemImageService)
        {
            _itemImageService = itemImageService;
        }

        // ======================================================
        // UPLOAD IMAGE FILE (NO DB)
        // ======================================================
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Upload image file and return its URL")]
        public async Task<IActionResult> Upload([FromForm] ItemImageUploadDTO dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No file uploaded"
                });
            }

            var rootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "items"
            );

            Directory.CreateDirectory(rootPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var fullPath = Path.Combine(rootPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var url = $"/items/{fileName}";

            return Ok(new
            {
                success = true,
                url
            });
        }


        // ======================================================
        // CREATE IMAGE RECORD (DB)
        // ======================================================
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new image for the item")]
        public async Task<IActionResult> Create([FromBody] ItemImageDTO dto)
        {
            var id = await _itemImageService.CreateAsync(dto);

            if (id <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Image not created"
                });
            }

            dto.ItemId = id;

            return Ok(new
            {
                success = true,
                data = dto
            });
        }

        // ======================================================
        // DELETE IMAGE (DB)
        // ======================================================
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an image")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _itemImageService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Image is deleted or not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Image deleted successfully",
                id
            });
        }

        // ======================================================
        // SET PRIMARY IMAGE
        // ======================================================
        [HttpPut("SetPrimary/{imageId}")]
        [SwaggerOperation(Summary = "Set an image as the primary image of its item")]
        public async Task<IActionResult> SetPrimary(int imageId)
        {
            var updated = await _itemImageService.SetAsPrimaryAsync(imageId);

            if (!updated)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Image not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Image set as primary successfully",
                imageId
            });
        }
    }
}

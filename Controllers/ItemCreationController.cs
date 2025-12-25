using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCreationController : ControllerBase
    {
        private readonly IItemCreationService _service;

        public ItemCreationController(IItemCreationService service)
        {
            _service = service;
        }

        [HttpPost("full")]
        public async Task<IActionResult> CreateFull([FromBody] ItemFullCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState
                });
            }

            var itemId = await _service.CreateFullItemAsync(dto);
            return Ok(new { success = true, itemId });
        }
    }
}

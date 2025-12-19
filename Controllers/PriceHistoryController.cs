using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/price-history")]
    public class PriceHistoryController : ControllerBase
    {
        private readonly IPriceHistoryService _service;

        public PriceHistoryController(IPriceHistoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PriceHistoryCreateDTO dto)
            => Ok(new
            {
                Id = await _service.CreatePriceHistoryAsync(dto),
                Message = "Historial creado correctamente"
            });

       

        [HttpGet("variant/{itemVariantId:int}")]
        public async Task<IActionResult> History(
            int itemVariantId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
            => Ok(await _service.GetPriceHistoryByVariantAsync(itemVariantId, from, to));

        [HttpGet("all")]
        public async Task<IActionResult> ListAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 20)
            => Ok(await _service.ListAllPriceHistoryAsync(search, page, limit));
    }
}

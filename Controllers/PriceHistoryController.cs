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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PriceHistoryUpdateDTO dto)
            => await _service.UpdatePriceHistoryAsync(id, dto)
                ? Ok()
                : NotFound();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _service.DeletePriceHistoryAsync(id)
                ? Ok()
                : NotFound();

        [HttpGet("current/price/{itemVariantId:int}")]
        public async Task<IActionResult> GetCurrentPrice(int itemVariantId)
            => Ok(await _service.GetCurrentSalePriceAsync(itemVariantId));

        [HttpGet("current/cost/{itemVariantId:int}")]
        public async Task<IActionResult> GetCurrentCost(int itemVariantId)
            => Ok(await _service.GetCurrentCostAsync(itemVariantId));

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

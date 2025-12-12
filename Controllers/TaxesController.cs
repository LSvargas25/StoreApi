using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Tax;
using StoreApi.ModelsDTO.Tax;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxesController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        //Create a new tax
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new tax.")]
        public async Task<IActionResult> CreateAsync(TaxDTO dto)
        {
            var newId = await _taxService.CreateAsync(dto);
            return Ok(new { message = "Tax created successfully.", taxId = newId });
        }


        //Delete a tax 
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a tax.")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _taxService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Tax with ID {id} not found." });

            return Ok(new { message = "Tax deleted successfully." });
        }

        //Get all Tax
        [HttpGet]
        [SwaggerOperation(Summary = "Get all taxes.")]
        public async Task<IActionResult> GetAllAsync(string? search, int page, int limit)
        {
            var list = await _taxService.GetAllAsync(search, page, limit);
            return Ok(list);
        }

        //Update a tax 
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a tax.")]
        public async Task<IActionResult> UpdateAsync(int id, TaxDTO dto)
        {
            if (id != dto.TaxId)
                return BadRequest(new { message = "Tax ID in URL does not match Tax ID in body." });

            var updated = await _taxService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Tax with ID {id} not found." });

            return Ok(new { message = "Tax updated successfully." });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Invoice;
using StoreApi.ModelsDTO.Customer;
using StoreApi.ModelsDTO.Invoice;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceTypesController : ControllerBase
    {
        private readonly IInvoiceTypeService _invoiceTypeService;

        public InvoiceTypesController(IInvoiceTypeService invoiceTypeService)
        {
            _invoiceTypeService = invoiceTypeService;
        }

        //Get all the Invoice Types 
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Invoice types.")]
        public async Task<ActionResult<List<InvoiceTypeDTO>>> Get([FromQuery] string? search, int page = 1, int limit = 10)
        {
            var types = await _invoiceTypeService.GetAllAsync(search, page, limit);
            return Ok(types);
        }

        //Create a new Invoice Type
        [HttpPost]
        [SwaggerOperation(Summary = "Create an Invoice Type.")]
        public async Task<IActionResult> Create([FromBody] InvoiceTypeCretDTO dto)
        {
            var id = await _invoiceTypeService.CreateAsync(dto);
            dto.InvoiceTypeId = id;

            return Ok(new
            {
                message = "Invoice type created successfully.",
                invoiceType = dto
            });
        }

        //Delete Invoice Type
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an Invoice Type.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _invoiceTypeService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Invoice type with ID {id} not found." });

            return Ok(new { message = "Invoice type deleted successfully.", id });
        }

        //Update an Invoice Type
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an Invoice Type.")]
        public async Task<IActionResult> Update(int id, [FromBody] InvoiceTypeCretDTO dto)
        {
            if (id != dto.InvoiceTypeId)
                return BadRequest(new { message = "InvoiceType ID in URL does not match ID in body." });

            var updated = await _invoiceTypeService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Invoice type with ID {id} not found." });

            return Ok(new { message = "Invoice type updated successfully." });
        }

        [HttpPut("{id}/status")]
        [SwaggerOperation(Summary = "Change invoice type status.")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] InvoiceChangeStatus dto)
        {
            if (id != dto.InvoiceTypeId)
                return BadRequest(new { message = "InvoiceType ID in URL does not match ID in body." });

            var updated = await _invoiceTypeService.ChangeStatusAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Invoice type with ID {id} not found." });

            return Ok(new { message = "Invoice type status updated successfully." });
        }



    }
}

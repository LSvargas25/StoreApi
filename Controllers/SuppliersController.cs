using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Supplier;
using StoreApi.ModelsDTO.Supplier;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: api/Suppliers?search=xxx
        [HttpGet]
        [SwaggerOperation(Summary = "Get all suppliers")]
        public async Task<ActionResult<List<SupplierDTO>>> GetSuppliers([FromQuery] string? search)
        {
            var list = await _supplierService.GetAllAsync(search);
            return Ok(list);
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get supplier by ID")]
        public async Task<ActionResult<SupplierDTO>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
                return NotFound(new { message = $"Supplier with ID {id} not found." });

            return Ok(supplier);
        }

        // POST: api/Suppliers
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new supplier")]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplier dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Validation Error", errors = ModelState });

            try
            {
                var id = await _supplierService.CreateAsync(dto);
                dto.SupplierId = id;
                return Ok(new { message = "Supplier created successfully.", data = dto });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update supplier")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierUpdate dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Validation Error", errors = ModelState });

            try
            {
                var updated = await _supplierService.UpdateAsync(id, dto);
                if (!updated)
                    return NotFound(new { message = $"Supplier with ID {id} not found." });

                return Ok(new { message = "Supplier updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete supplier")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var deleted = await _supplierService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Supplier with ID {id} not found." });

            return Ok(new { message = "Supplier deleted successfully." });
        }

        // PATCH: api/Suppliers/ChangeStatus/5
        [HttpPatch("ChangeStatus/{id}")]
        [SwaggerOperation(Summary = "Change supplier status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] SupplierStatus dto)
        {
            try
            {
                var updated = await _supplierService.ChangeStatus(id, dto);
                if (!updated)
                    return NotFound(new { message = $"Supplier with ID {id} not found." });

                return Ok(new { message = "Supplier status updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PATCH: api/Suppliers/ChangeRole/5
        [HttpPatch("ChangeRole/{id}")]
        [SwaggerOperation(Summary = "Change supplier role/type")]
        public async Task<IActionResult> ChangeRole(int id, [FromBody] SupplierRole dto)
        {
            try
            {
                var updated = await _supplierService.ChangeRole(id, dto);
                if (!updated)
                    return NotFound(new { message = $"Supplier with ID {id} not found." });

                return Ok(new { message = "Supplier type updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

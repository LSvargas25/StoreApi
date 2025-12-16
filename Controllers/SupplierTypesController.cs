using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Interface.Supplier;
using StoreApi.Interface.User;
using StoreApi.Models;
using StoreApi.ModelsDTO.Supplier;
using StoreApi.ModelsDTO.User;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Supplier")]
    public class SupplierTypesController : ControllerBase
    {
        private readonly ISupplierTypeService _supplierService;

        public SupplierTypesController(ISupplierTypeService supplierService)
        {
            _supplierService = supplierService;
        }

        //Gell all the Supplier types
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Supplier types.")]

        public async Task<ActionResult<List<SupplierTypeDTO>>> Get([FromQuery] string? search, int page = 1, int limit = 10)
        {
            var types = await _supplierService.GetAllAsync(search, page, limit);
            return Ok(types);
        }
        //Create the suppliertype
        [HttpPost]
        [SwaggerOperation(Summary = "Create a Supplier type.")]
        public async Task<IActionResult> Create([FromBody] SupplierTypeDTO dto)
        {
            var id = await _supplierService.CreateAsync(dto);
            dto.SupplierTypeId = id;
            return Ok(dto);
        }
        //Delete supplier 
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Supplier type.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _supplierService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Supplier type not found" });

            return Ok(new { message = "Supplier type deleted successfully", id });
        }
        //Update an Supplier type
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an Supplier type.")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierTypeDTO dto)
        {
            if (id != dto.SupplierTypeId)
                return BadRequest(new { message = "Supplier type ID in URL does not match ID in body." });

            var updated = await _supplierService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Supplier type type with ID {id} not found." });

            return Ok(new { message = "Supplier type updated successfully." });
        }


        // GET: api/roles/names
        [HttpGet("names")]
        [SwaggerOperation(Summary = "Get the Role name by id")]
        public async Task<IActionResult> GetRoleNames()
        {
            var roles = await _supplierService.GetRoleNamesAsync();
            return Ok(roles);
        }


    }
}


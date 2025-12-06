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
    public class SupplierTypesController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierTypesController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        //Gell all the Supplier types
        [HttpGet]
        [SwaggerOperation(Summary = "Get all the supplier type on the bd ")]
        public async Task<ActionResult<List<SupplierDTO>>> Get([FromQuery] string? search)
        {
            var users = await _supplierService.GetAllAsync(search);
            return Ok(users);
        }

        //Create a new Supplier type
        [HttpPost]
        [SwaggerOperation(Summary = "Register a new  Supplier tyoe")]
        public async Task<IActionResult> Create([FromBody] SupplierDTO dto)
        {
            var id = await _supplierService.CreateAsync(dto);

            dto.SupplierId = id;

            return Ok(dto);
        }

        //Delete  a supplier type
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete SupplierType")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _supplierService.DeleteAsync(id);
            if (!deleted) return NotFound();

            // Return 200 with a simple message
            return Ok(new { message = "Role deleted successfully", SupplierId = id });
        }



    }
}

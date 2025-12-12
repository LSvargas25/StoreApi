using Microsoft.AspNetCore.Mvc;
using StoreApi.Interface.Customer;
using StoreApi.ModelsDTO.Customer;
 
using Swashbuckle.AspNetCore.Annotations;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRolesController : ControllerBase
    {
        private readonly ICustomerRoleService _customerRoleService;

        public CustomerRolesController(ICustomerRoleService customerRoleService )
        {
            _customerRoleService = customerRoleService;
        }

        //Gell all the Customer types
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Customer types.")]

        public async Task<ActionResult<List<CustomerRoleDTO>>> Get([FromQuery] string? search, int page = 1, int limit = 10)
        {
            var types = await _customerRoleService.GetAllAsync(search, page, limit);
            return Ok(types);
        }
        //Create the Customer types
        [HttpPost]
        [SwaggerOperation(Summary = "Create a Customer type.")]
        public async Task<IActionResult> Create([FromBody] CustomerRoleCreat dto)
        {
            var id = await _customerRoleService.CreateAsync(dto);
            dto.CustomerRoleId= id;
            return Ok(dto);
        }
        //Delete Customer
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a Customer type.")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _customerRoleService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Customer Type type not found" });

            return Ok(new { message = "Customer type deleted successfully", id });
        }
        // UPDATE CUSTOMER ROLE NAME
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Customer Role name")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerRoleUpdt dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            var updated = await _customerRoleService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Customer Role with ID {id} not found." });

            return Ok(new { message = "Customer Role updated successfully." });
        }

        // CHANGE CUSTOMER ROLE STATUS
        [HttpPut("{id}/status")]
        [SwaggerOperation(Summary = "Change active status of a Customer Role")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] CustomerRoleChangs dto)
        {
            var updated = await _customerRoleService.ChangeStatusAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Customer Role with ID {id} not found." });

            return Ok(new { message = "Customer Role status updated successfully." });
        }




    }


}
 

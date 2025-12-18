using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private readonly IItemAttributeService _attributeService;

        public AttributesController(IItemAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        // ======================================================
        // GET ALL
        // ======================================================
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all attributes",
            Description = "Retrieves all available attributes. Optionally filters results by a search term."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AttributeDTO>>> GetAll(
            [FromQuery] string? search
        )
        {
            var result = await _attributeService.GetAllAttributesAsync(search);
            return Ok(result);
        }

        // ======================================================
        // CREATE
        // ======================================================
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new attribute",
            Description = "Creates a new attribute and returns the generated attribute identifier."
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] AttributeCreateDTO dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Invalid attribute data",
                    errors = ModelState
                });

            var newId = await _attributeService.CreateAttributeAsync(dto);

            return CreatedAtAction(
                nameof(GetAll),
                new { id = newId },
                new
                {
                    message = "Attribute created successfully",
                    attributeId = newId
                }
            );
        }

        // ======================================================
        // UPDATE
        // ======================================================
        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Update an attribute",
            Description = "Updates an existing attribute identified by its ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] AttributeCreateDTO dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Invalid attribute data",
                    errors = ModelState
                });

            var updated = await _attributeService.UpdateAttributeAsync(id, dto);

            if (!updated)
                return NotFound(new
                {
                    message = $"Attribute with ID {id} was not found"
                });

            return Ok(new
            {
                message = "Attribute updated successfully",
                attributeId = id
            });
        }

        // ======================================================
        // DELETE
        // ======================================================
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete an attribute",
            Description = "Deletes an attribute by its ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _attributeService.DeleteAttributeAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    message = $"Attribute with ID {id} was not found"
                });

            return Ok(new
            {
                message = "Attribute deleted successfully",
                attributeId = id
            });
        }
    }
}

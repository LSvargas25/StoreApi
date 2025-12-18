using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/items/{itemId:int}/attributes")]
    public class ItemAttributeDetailsController : ControllerBase
    {
        private readonly IItemAttributeDetailService _service;

        public ItemAttributeDetailsController(IItemAttributeDetailService service)
        {
            _service = service;
        }

        // ======================================================
        // GET ALL ATTRIBUTES BY ITEM
        // ======================================================
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieve all attributes associated with a specific item."
        )]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromRoute] int itemId)
        {
            var result = await _service.GetAllAttributeDetailsByItemIdAsync(itemId);

            return Ok(new
            {
                message = "Attributes retrieved successfully.",
                data = result
            });
        }

        // ======================================================
        // CREATE ATTRIBUTE DETAIL
        // ======================================================
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new attribute for a specific item."
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromRoute] int itemId,
            [FromBody] AttributeCreateDetailDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Invalid request data.",
                    errors = ModelState
                });

            var success = await _service.CreateAttributeDetailAsync(itemId, dto);

            if (!success)
                return BadRequest(new
                {
                    message = "Attribute was not created."
                });

            return CreatedAtAction(
                nameof(GetAll),
                new { itemId },
                new
                {
                    message = "Attribute created successfully."
                });
        }

        // ======================================================
        // UPDATE ATTRIBUTE DETAIL
        // ======================================================
        [HttpPut("{attributeId:int}")]
        [SwaggerOperation(
            Summary = "Update an attribute value for a specific item."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] int itemId,
            [FromRoute] int attributeId,
            [FromBody] AttributeCreateDetailDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Invalid request data.",
                    errors = ModelState
                });

            var success = await _service.UpdateAttributeDetailAsync(
                itemId,
                attributeId,
                dto);

            if (!success)
                return NotFound(new
                {
                    message = "Attribute detail not found."
                });

            return Ok(new
            {
                message = "Attribute updated successfully."
            });
        }

        // ======================================================
        // DELETE ATTRIBUTE DETAIL
        // ======================================================
        [HttpDelete("{attributeId:int}")]
        [SwaggerOperation(
            Summary = "Delete an attribute from a specific item."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] int itemId,
            [FromRoute] int attributeId)
        {
            var success = await _service.DeleteAttributeDetailAsync(itemId, attributeId);

            if (!success)
                return NotFound(new
                {
                    message = "Attribute detail not found."
                });

            return Ok(new
            {
                message = "Attribute deleted successfully."
            });
        }

        // ======================================================
        // CHANGE FAVORITE STATUS
        // ======================================================
        [HttpPatch("favorite")]
        [SwaggerOperation(
            Summary = "Mark or unmark an attribute as favorite for a specific item."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeFavoriteStatus(
            [FromRoute] int itemId,
            [FromBody] ChangeFavoriteStatusDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Invalid request data.",
                    errors = ModelState
                });

            var success = await _service.ChangeFavoriteStatusAsync(itemId, dto);

            if (!success)
                return NotFound(new
                {
                    message = "Attribute detail not found."
                });

            return Ok(new
            {
                message = "Favorite status updated successfully."
            });
        }
    }
}

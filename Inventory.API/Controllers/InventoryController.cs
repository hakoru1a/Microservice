using Microsoft.AspNetCore.Mvc;
using Inventory.API.Services.Interfaces;
using Shared.DTOs.Inventory;
using Shared.SeedWork;
using Inventory.API.Entities;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IIventoryService _inventoryService;

        public InventoryController(IIventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InventoryEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryEntryDto>> GetById(string id)
        {
            try
            {
                var result = await _inventoryService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex) when (ex.Message == "Inventory entry not found")
            {
                return NotFound();
            }
        }

        [HttpGet("items/{itemNo}")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetByItemNo(string itemNo)
        {
            var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        [HttpGet("items/{itemNo}/paged")]
        [ProducesResponseType(typeof(PagedList<InventoryEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedList<InventoryEntryDto>>> GetPagedByItemNo(
            string itemNo,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            var query = new GetInventoryPagingQuery
            {
                ItemNo = itemNo,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm
            };

            var result = await _inventoryService.GetPageByItemNoAsync(query);
            return Ok(result);
        }

        [HttpPost("purchase/{itemNo}")]
        [ProducesResponseType(typeof(InventoryEntryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseProduct(
            string itemNo,
            [FromBody] PurchaseProductDto purchaseProductDto)
        {
            if (itemNo != purchaseProductDto.ItemNo)
                return BadRequest("ItemNo in URL must match ItemNo in request body");

            var result = await _inventoryService.PurchaseProduct(itemNo, purchaseProductDto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            await _inventoryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string id, [FromBody] InventoryEntry entity)
        {
            if (id != entity.Id)
                return BadRequest("Id in URL must match Id in request body");

            await _inventoryService.UpdateAsync(entity);
            return NoContent();
        }
    }
}
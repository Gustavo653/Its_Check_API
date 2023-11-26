using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    public class ChecklistReplacedItemController : BaseController
    {
        private readonly IChecklistReplacedItemService _ChecklistReplacedItemService;

        public ChecklistReplacedItemController(IChecklistReplacedItemService ChecklistReplacedItemService)
        {
            _ChecklistReplacedItemService = ChecklistReplacedItemService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateChecklistReplacedItem([FromBody] ChecklistReplacedItemDTO ChecklistReplacedItemDTO)
        {
            var ChecklistReplacedItem = await _ChecklistReplacedItemService.Create(ChecklistReplacedItemDTO);
            return StatusCode(ChecklistReplacedItem.Code, ChecklistReplacedItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklistReplacedItem([FromRoute] int id, [FromBody] ChecklistReplacedItemDTO ChecklistReplacedItemDTO)
        {
            var ChecklistReplacedItem = await _ChecklistReplacedItemService.Update(id, ChecklistReplacedItemDTO);
            return StatusCode(ChecklistReplacedItem.Code, ChecklistReplacedItem);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveChecklistReplacedItem([FromRoute] int id)
        {
            var ChecklistReplacedItem = await _ChecklistReplacedItemService.Remove(id);
            return StatusCode(ChecklistReplacedItem.Code, ChecklistReplacedItem);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetChecklistReplacedItems()
        {
            var ChecklistReplacedItem = await _ChecklistReplacedItemService.GetList();
            return StatusCode(ChecklistReplacedItem.Code, ChecklistReplacedItem);
        }
    }
}
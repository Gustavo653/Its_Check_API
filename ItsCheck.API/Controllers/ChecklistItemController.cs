using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    public class ChecklistItemController : BaseController
    {
        private readonly IChecklistItemService _checklistItemService;

        public ChecklistItemController(IChecklistItemService checklistItemService)
        {
            _checklistItemService = checklistItemService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateChecklistItem([FromBody] ChecklistItemDTO checklistItemDTO)
        {
            var checklistItem = await _checklistItemService.Create(checklistItemDTO);
            return StatusCode(checklistItem.Code, checklistItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklistItem([FromRoute] int id, [FromBody] ChecklistItemDTO checklistItemDTO)
        {
            var checklistItem = await _checklistItemService.Update(id, checklistItemDTO);
            return StatusCode(checklistItem.Code, checklistItem);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveChecklistItem([FromRoute] int id)
        {
            var checklistItem = await _checklistItemService.Remove(id);
            return StatusCode(checklistItem.Code, checklistItem);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetChecklistItems()
        {
            var checklistItem = await _checklistItemService.GetList();
            return StatusCode(checklistItem.Code, checklistItem);
        }
    }
}
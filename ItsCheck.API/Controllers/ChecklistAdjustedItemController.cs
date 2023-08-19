using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    public class ChecklistAdjustedItemController : BaseController
    {
        private readonly IChecklistAdjustedItemService _checklistAdjustedItemService;

        public ChecklistAdjustedItemController(IChecklistAdjustedItemService checklistAdjustedItemService)
        {
            _checklistAdjustedItemService = checklistAdjustedItemService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateChecklistAdjustedItem([FromBody] ChecklistAdjustedItemDTO checklistAdjustedItemDTO)
        {
            var checklistAdjustedItem = await _checklistAdjustedItemService.Create(checklistAdjustedItemDTO);
            return StatusCode(checklistAdjustedItem.Code, checklistAdjustedItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklistAdjustedItem([FromRoute] int id, [FromBody] ChecklistAdjustedItemDTO checklistAdjustedItemDTO)
        {
            var checklistAdjustedItem = await _checklistAdjustedItemService.Update(id, checklistAdjustedItemDTO);
            return StatusCode(checklistAdjustedItem.Code, checklistAdjustedItem);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveChecklistAdjustedItem([FromRoute] int id)
        {
            var checklistAdjustedItem = await _checklistAdjustedItemService.Remove(id);
            return StatusCode(checklistAdjustedItem.Code, checklistAdjustedItem);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetChecklistAdjustedItems()
        {
            var checklistAdjustedItem = await _checklistAdjustedItemService.GetList();
            return StatusCode(checklistAdjustedItem.Code, checklistAdjustedItem);
        }
    }
}
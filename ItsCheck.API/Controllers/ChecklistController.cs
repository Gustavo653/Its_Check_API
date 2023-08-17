using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;

namespace ItsCheck.API.Controllers
{
    public class ChecklistController : BaseController
    {
        private readonly IChecklistService _checklistService;

        public ChecklistController(IChecklistService checklistService)
        {
            _checklistService = checklistService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateChecklist([FromBody] BasicDTO name)
        {
            var checklist = await _checklistService.Create(name);
            return StatusCode(checklist.Code, checklist);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklist([FromRoute] int id, [FromBody] BasicDTO name)
        {
            var checklist = await _checklistService.Update(id, name);
            return StatusCode(checklist.Code, checklist);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveChecklist([FromRoute] int id)
        {
            var checklist = await _checklistService.Remove(id);
            return StatusCode(checklist.Code, checklist);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetChecklists()
        {
            var checklist = await _checklistService.GetList();
            return StatusCode(checklist.Code, checklist);
        }
    }
}
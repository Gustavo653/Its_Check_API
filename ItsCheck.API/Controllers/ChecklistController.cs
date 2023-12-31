using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Infrastructure.Service;
using ItsCheck.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateChecklist([FromBody] ChecklistDTO checklistDTO)
        {
            var checklist = await _checklistService.Create(checklistDTO);
            return StatusCode(checklist.Code, checklist);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklist([FromRoute] int id, [FromBody] ChecklistDTO checklistDTO)
        {
            var checklist = await _checklistService.Update(id, checklistDTO);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var checklist = await _checklistService.GetById(id);
            return StatusCode(checklist.Code, checklist);
        }
        
        [HttpGet("ByUserAmbulance")]
        public async Task<IActionResult> GetByAmbulanceId()
        {
            var checklist = await _checklistService.GetByAmbulanceId(Convert.ToInt32(ClaimsPrincipalExtensions.GetUserId(User)));
            return StatusCode(checklist.Code, checklist);
        }
    }
}
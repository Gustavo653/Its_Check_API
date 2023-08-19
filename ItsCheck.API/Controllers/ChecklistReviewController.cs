using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;

namespace ItsCheck.API.Controllers
{
    public class ChecklistReviewController : BaseController
    {
        private readonly IChecklistReviewService _checklistReviewService;

        public ChecklistReviewController(IChecklistReviewService checklistReviewService)
        {
            _checklistReviewService = checklistReviewService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateChecklistReview([FromBody] ChecklistReviewDTO checklistReviewDTO)
        {
            var checklistReview = await _checklistReviewService.Create(checklistReviewDTO);
            return StatusCode(checklistReview.Code, checklistReview);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklistReview([FromRoute] int id, [FromBody] ChecklistReviewDTO checklistReviewDTO)
        {
            var checklistReview = await _checklistReviewService.Update(id, checklistReviewDTO);
            return StatusCode(checklistReview.Code, checklistReview);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveChecklistReview([FromRoute] int id)
        {
            var checklistReview = await _checklistReviewService.Remove(id);
            return StatusCode(checklistReview.Code, checklistReview);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCategories()
        {
            var checklistReview = await _checklistReviewService.GetList();
            return StatusCode(checklistReview.Code, checklistReview);
        }
    }
}
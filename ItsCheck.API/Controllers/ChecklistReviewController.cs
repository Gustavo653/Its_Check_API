using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Infrastructure.Service;
using ItsCheck.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateChecklistReview([FromBody] ChecklistReviewDTO checklistReviewDTO)
        {
            checklistReviewDTO.IdUser = Convert.ToInt32(ClaimsPrincipalExtensions.GetUserId(User));
            var checklistReview = await _checklistReviewService.Create(checklistReviewDTO);
            return StatusCode(checklistReview.Code, checklistReview);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateChecklistReview([FromRoute] int id, [FromBody] ChecklistReviewDTO checklistReviewDTO)
        {
            checklistReviewDTO.IdUser = Convert.ToInt32(ClaimsPrincipalExtensions.GetUserId(User));
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
        public async Task<IActionResult> GetChecklistReviews([FromQuery] int? takeLast)
        {
            var checklistReview = await _checklistReviewService.GetList(takeLast);
            return StatusCode(checklistReview.Code, checklistReview);
        }

        [HttpGet("ExistsInitialChecklistReview")]
        public async Task<IActionResult> ExistsChecklistReview()
        {
            var checklistReview = await _checklistReviewService.ExistsChecklistReview(Convert.ToInt32(User.GetUserId()));
            return StatusCode(checklistReview.Code, checklistReview);
        }
    }
}
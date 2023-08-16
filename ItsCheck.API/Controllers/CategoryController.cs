using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;

namespace ItsCheck.API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateCategory([FromBody] BasicDTO name)
        {
            var category = await _categoryService.CreateCategory(name);
            return StatusCode(category.Code, category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] BasicDTO name)
        {
            var category = await _categoryService.UpdateCategory(id, name);
            return StatusCode(category.Code, category);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveCategory([FromRoute] int id)
        {
            var category = await _categoryService.RemoveCategory(id);
            return StatusCode(category.Code, category);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCategories()
        {
            var category = await _categoryService.GetCategories();
            return StatusCode(category.Code, category);
        }
    }
}
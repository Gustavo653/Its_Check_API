using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var category = await _categoryService.Create(name);
            return StatusCode(category.Code, category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] BasicDTO name)
        {
            var category = await _categoryService.Update(id, name);
            return StatusCode(category.Code, category);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveCategory([FromRoute] int id)
        {
            var category = await _categoryService.Remove(id);
            return StatusCode(category.Code, category);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCategories()
        {
            var category = await _categoryService.GetList();
            return StatusCode(category.Code, category);
        }
    }
}
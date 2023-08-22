using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateItem([FromBody] BasicDTO name)
        {
            var item = await _itemService.Create(name);
            return StatusCode(item.Code, item);
        }
        
        [HttpPost("Import")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> ImportCSV([FromForm] IFormFile csvFile)
        {
            var importResult = await _itemService.ImportCSV(csvFile);
            return StatusCode(importResult.Code, importResult);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] BasicDTO name)
        {
            var item = await _itemService.Update(id, name);
            return StatusCode(item.Code, item);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveItem([FromRoute] int id)
        {
            var item = await _itemService.Remove(id);
            return StatusCode(item.Code, item);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetItems()
        {
            var item = await _itemService.GetList();
            return StatusCode(item.Code, item);
        }
    }
}
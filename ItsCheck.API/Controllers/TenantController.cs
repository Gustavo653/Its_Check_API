using ItsCheck.Domain.Enum;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    public class TenantController : BaseController
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateTenant([FromBody] BasicDTO name)
        {
            var item = await _tenantService.Create(name);
            return StatusCode(item.Code, item);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateTenant([FromRoute] int id, [FromBody] BasicDTO name)
        {
            var item = await _tenantService.Update(id, name);
            return StatusCode(item.Code, item);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveTenant([FromRoute] int id)
        {
            var item = await _tenantService.Remove(id);
            return StatusCode(item.Code, item);
        }

        [HttpGet("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> GetTenants()
        {
            var item = await _tenantService.GetList();
            return StatusCode(item.Code, item);
        }
    }
}
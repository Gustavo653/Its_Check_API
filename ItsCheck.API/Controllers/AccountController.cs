using Common.Functions;
using ItsCheck.Domain.Enum;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Current")]
        public async Task<IActionResult> GetUser()
        {
            var user = await _accountService.GetCurrent(User.GetEmail());
            return StatusCode(user.Code, user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            var user = await _accountService.Login(userLogin);
            return StatusCode(user.Code, user);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _accountService.GetUsers();
            return StatusCode(user.Code, user);
        }

        [HttpPost("")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            var user = await _accountService.CreateUser(userDTO);
            return StatusCode(user.Code, user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDTO userDTO)
        {
            var user = await _accountService.UpdateUser(id, userDTO);
            return StatusCode(user.Code, user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(RoleName.Admin))]
        public async Task<IActionResult> RemoveUser([FromRoute] int id)
        {
            var user = await _accountService.RemoveUser(id);
            return StatusCode(user.Code, user);
        }
    }
}
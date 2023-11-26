using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsCheck.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    //[Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}

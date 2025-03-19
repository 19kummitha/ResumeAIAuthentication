using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Resumecontroller : ControllerBase
    {
        [HttpGet]
        public IActionResult GetResume()
        {
            return Ok("Resume printed successfully");
        }
    }
}

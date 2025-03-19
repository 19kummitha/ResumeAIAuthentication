using API.Model;
using API.TokenManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthManager authManager) : ControllerBase
    {
        private readonly IAuthManager _authManager = authManager;

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            return await _authManager.Login(model);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            return await _authManager.Register(model);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register model)
        {
            return await _authManager.RegisterAdmin(model);

        }
    }
}

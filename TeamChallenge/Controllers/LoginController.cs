using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Models.Requests.Login;
using TeamChallenge.Models.Responses;
using TeamChallenge.Services;

namespace TeamChallenge.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IResponse> Login([FromBody] TCLoginRequest request)
        {
            return await _service.Login(request);
        }

        [HttpPost("signup")]
        public async Task<IResponse> SignUp([FromBody] SignUpRequest request)
        {
            return await _service.SignUp(request);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IResponse> Logout()
        {
           return await _service.Logout();
        }

        [HttpGet("confirm-email")]
        public async Task<IResponse> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            return await _service.ConfirmEmail(email, token);
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IResponse> ResendEmailConfirmation([FromQuery] string email, [FromQuery] string clientUrl)
        {
            return await _service.ResendEmailConfirmation(email, clientUrl);
        }
    }
}

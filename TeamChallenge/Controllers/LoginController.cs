using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Services;

namespace TeamChallenge.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController(ILoginService service) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IResponse> Login([FromBody] TCLoginRequest request)
        {
            return await service.Login(request);
        }

        [HttpPost("signup")]
        public async Task<IResponse> SignUp([FromBody] SignUpRequest request)
        {
            return await service.SignUp(request);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IResponse> Logout()
        {
           return await service.Logout();
        }

        [HttpGet("confirm-email")]
        public async Task<IResponse> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            return await service.ConfirmEmail(email, token);
        }

        [HttpPost("resend-email-confirmation")]
        public async Task<IResponse> ResendEmailConfirmation([FromBody]ResendEmailConfirmationRequest request)
        {
            return await service.ResendEmailConfirmation(request);
        }
    }
}

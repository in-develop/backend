using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Models.Responses;
using TeamChallenge.Services;

[ApiController]
[Route("api/auth")]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleOAuth _googleOAuthService;

    public GoogleAuthController(IGoogleOAuth googleOAuthService)
    {
        _googleOAuthService = googleOAuthService;
    }

    [HttpGet("google-signin")]
    public async Task<IResponse> GetGoogleLoginUrl()
    {
        return _googleOAuthService.GenerateOAuthRequestUrl();
    }

    [HttpGet("google-callback")]
    public async Task<IResponse> GoogleCallback([FromQuery] string code, [FromQuery] string state)
    {
        return await _googleOAuthService.GetGoogleAuthCallback(code, state);
    }

}

using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Models.Responses;
using TeamChallenge.Services;

namespace TeamChallenge.Controllers;

[ApiController]
[Route("api/auth")]
public class GoogleAuthController(IGoogleOAuth googleOAuthService) : ControllerBase
{
    [HttpGet("google-signin")]
    public Task<IResponse> GetGoogleLoginUrl()
    {
        return Task.FromResult(googleOAuthService.GenerateOAuthRequestUrl());
    }

    [HttpGet("google-callback")]
    public async Task<IResponse> GoogleCallback([FromQuery] string code, [FromQuery] string state)
    {
        return await googleOAuthService.GetGoogleAuthCallback(code, state);
    }
}
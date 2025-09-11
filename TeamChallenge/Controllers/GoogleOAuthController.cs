using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Responses;
using TeamChallenge.Services;

[ApiController]
[Route("api/auth")]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleOAuth _googleOAuthService;
    private readonly IConfiguration _configuration;

    public GoogleAuthController(IGoogleOAuth googleOAuthService, IConfiguration configuration)
    {
        _googleOAuthService = googleOAuthService;
        _configuration = configuration;
    }

    [HttpGet("google-signin")]
    public async Task<ActionResult<IDataResponse<string>>> GetGoogleLoginUrl()
    {
        var scope = "openid";
        var redirectUri = _configuration["Authentication:Google:RedirectUri"];
        string codeVerifier, state, codeChallenge;

        _googleOAuthService.GenerateCodeVerifierState(out codeVerifier, out state, out codeChallenge);
        HttpContext.Session.SetString("code_verifier", codeVerifier);
        HttpContext.Session.SetString("oauth_state", state);
        var response = _googleOAuthService.GenerateOAuthRequestUrl(scope, redirectUri!, codeChallenge, state);

        return Ok(response);
    }

    [HttpGet("google-callback")]
    public async Task<ActionResult<IDataResponse<GoogleAuthCallback>>> GoogleCallback([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest(new ErrorResponse("Authorization code missing."));
        }

        var savedState = HttpContext.Session.GetString("oauth_state");
        if (state != savedState)
        {
            return BadRequest(new ErrorResponse("Invalid state value."));
        }

        var response = await _googleOAuthService.GetGoogleAuthCallback(code);

        return Ok(response);
    }

}

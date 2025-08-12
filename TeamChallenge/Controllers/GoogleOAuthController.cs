using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Helpers;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.Responses;

[ApiController]
[Route("api/account")]
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
    public IActionResult GetGoogleLoginUrl()
    {
        var scope = "openid"; // your required scopes
        var redirectUri = _configuration["Authentication:Google:RedirectUri"];

        // PKCE support (optional, for better security)
        var codeVerifier = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("code_verifier", codeVerifier);
        
        var state = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("oauth_state", state);

        var codeChallenge = PkceHelper.GenerateCodeChallenge(codeVerifier);

        var url = _googleOAuthService.GenerateOAuthRequestUrl(scope, redirectUri, codeChallenge, state);

        return Ok(new { url });
    }


    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest("Authorization code missing.");

        var savedState = HttpContext.Session.GetString("oauth_state");
        if (state != savedState)
            return BadRequest("Invalid state value.");

        var redirectUri = _configuration["Authentication:Google:RedirectUri"];
        var codeVerifier = HttpContext.Session.GetString("code_verifier");

        var tokenResponse = await _googleOAuthService.ExchangeCodeOnToken(code, codeVerifier, redirectUri);
        var refreshToken = await _googleOAuthService.RefreshToken(tokenResponse.RefreshToken);

        return Ok(new GoogleCallbackResponse
        {
            IsSucceded = true,
            AuthGoogleResponse = tokenResponse,
            TokenResponse = refreshToken
        });
    }

}

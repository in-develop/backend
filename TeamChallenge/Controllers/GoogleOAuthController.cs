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
        var scope = "openid"; // your required scopes
        var redirectUri = _configuration["Authentication:Google:RedirectUri"];

        // TODO: Move this values inside service instead of passing them as parameters,
        // PKCE support (optional, for better security)
        var codeVerifier = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("code_verifier", codeVerifier);

        // TODO: Move this values inside service instead of passing them as parameters,
        var state = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("oauth_state", state);

        // TODO: Move this values inside service instead of passing them as parameters,
        var codeChallenge = PkceHelper.GenerateCodeChallenge(codeVerifier);

        var response = _googleOAuthService.GenerateOAuthRequestUrl(scope, redirectUri, codeChallenge, state);

        return Ok(response);
    }

#warning Check redirection
    [HttpGet("google-callback")]
    public async Task<ActionResult<IDataResponse<GoogleAuthCallback>>> GoogleCallback([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest(new ErrorResponse("Authorization code missing."));

        var savedState = HttpContext.Session.GetString("oauth_state");
        if (state != savedState)
            return BadRequest(new ErrorResponse("Invalid state value."));

        // TODO: Move this values inside service instead of passing them as parameters, controller should containg only input validation
        //       Retrieve httpContext from DI container, also you already have IConfiguration injected

        var redirectUri = _configuration["Authentication:Google:RedirectUri"];
        var codeVerifier = HttpContext.Session.GetString("code_verifier");

        var response = await _googleOAuthService.GetGoogleAuthCallback(code, codeVerifier, redirectUri);

        return Ok(response);
    }

}

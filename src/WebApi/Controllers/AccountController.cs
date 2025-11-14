using CleanSSO.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanSSO.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AuthService _authService;

    public AccountController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("login-google")]
    public IActionResult LoginGoogle(string? returnUrl = "/")
    {
        var props = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse), new { returnUrl }) };
        return Challenge(props, "Google");
    }

    [HttpGet("login-facebook")]
    public IActionResult LoginFacebook(string? returnUrl = "/")
    {
        var props = new AuthenticationProperties { RedirectUri = Url.Action(nameof(FacebookResponse), new { returnUrl }) };
        return Challenge(props, "Facebook");
    }

    [HttpGet("GoogleResponse")]
    public async Task<IActionResult> GoogleResponse(string? returnUrl = "/")
    {
        var external = await HttpContext.AuthenticateAsync();
        var claims = external.Principal?.Claims;

        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var providerKey = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var picture = claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

        if (email == null || providerKey == null)
            return BadRequest("Missing claims from external provider");

        var resp = await _authService.HandleExternalLoginAsync("google", providerKey, email, name, picture);
        return Ok(resp);
    }

    [HttpGet("FacebookResponse")]
    public async Task<IActionResult> FacebookResponse(string? returnUrl = "/")
    {
        var external = await HttpContext.AuthenticateAsync();
        var claims = external.Principal?.Claims;

        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var providerKey = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var picture = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;

        if (email == null || providerKey == null)
            return BadRequest("Missing claims from external provider");

        var resp = await _authService.HandleExternalLoginAsync("facebook", providerKey, email, name, picture);
        return Ok(resp);
    }
}

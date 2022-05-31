using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using WoodenWorkshop.PublicApi.Web.Core.Models.Auth;
using WoodenWorkshop.PublicApi.Web.Core.Options;
using WoodenWorkshop.PublicApi.Web.Core.Services;

namespace WoodenWorkshop.PublicApi.Web.Controllers;

[ApiController, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly WebApiAuthService _webApiAuthService;

    private readonly IOptionsSnapshot<SecurityOptions> _securityOptions;

    private string ClientIpAddress => HttpContext.Connection.RemoteIpAddress?.ToString()
                                      ?? throw new Exception("Произошла ошибка при попытке определить IP адрес пользователя.");
    
    private string RefreshTokenCookieName => _securityOptions.Value.RefreshTokenCookieName;

    public AuthController(WebApiAuthService webApiAuthService, IOptionsSnapshot<SecurityOptions> securityOptions)
    {
        _webApiAuthService = webApiAuthService;
        _securityOptions = securityOptions;
    }

    [HttpPost("")]
    public async Task<ActionResult<AuthResult>> AuthorizeAsync(AuthRequest authRequest)
    {
        var authResult = await _webApiAuthService.AuthorizeAsync(authRequest, ClientIpAddress);
        SetRefreshTokenCookie(authResult.RefreshToken);
        return Ok(authResult);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResult>> RefreshSessionAsync()
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            return Unauthorized();
        }

        var authResult = await _webApiAuthService.RefreshSessionAsync(refreshToken!);
        SetRefreshTokenCookie(authResult.RefreshToken);
        return Ok(authResult);
    }

    [HttpPost("sign-out")]
    public async Task<NoContentResult> SignOutAsync()
    {
        if (
            Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken)
                && !string.IsNullOrEmpty(refreshToken)
        )
        {
            await _webApiAuthService.TerminateSessionAsync(refreshToken);
        }

        return NoContent();
    }

    private void SetRefreshTokenCookie(RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Secure = _securityOptions.Value.SecureCookieEnabled,
            SameSite = SameSiteMode.Strict,
            Expires = refreshToken.ExpireDate,
            HttpOnly = true,
        };

        var cookieDomain = _securityOptions.Value.RefreshTokenCookieDomain;
        if (!string.IsNullOrEmpty(cookieDomain))
        {
            cookieOptions.Domain = cookieDomain;
        }
        
        Response.Cookies.Append(
            RefreshTokenCookieName,
            refreshToken.Token,
            cookieOptions
        );
    }
}
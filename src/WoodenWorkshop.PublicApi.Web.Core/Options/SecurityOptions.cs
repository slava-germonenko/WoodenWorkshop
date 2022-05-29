namespace WoodenWorkshop.PublicApi.Web.Core.Options;

public record SecurityOptions
{
    public int AccessTokenTtlSeconds { get; set; }

    public int RefreshTokenTtlMinutes { get; set; }

    public string JwtSecret { get; set; } = string.Empty;

    public string RefreshTokenCookieName { get; set; } = string.Empty;
    
    public string? RefreshTokenCookieDomain { get; set; }

    public bool SecureCookieEnabled { get; set; } = false;
}
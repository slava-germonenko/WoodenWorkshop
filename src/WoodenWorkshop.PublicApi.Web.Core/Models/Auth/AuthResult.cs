namespace WoodenWorkshop.PublicApi.Web.Core.Models.Auth;

public class AuthResult
{
    public AccessToken AccessToken { get; }
    
    public RefreshToken RefreshToken { get; }

    public AuthResult(AccessToken accessToken, RefreshToken refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
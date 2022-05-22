using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Extensions;
using WoodenWorkshop.PublicApi.Web.Core.Models.Auth;
using WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;
using WoodenWorkshop.PublicApi.Web.Core.Options;

namespace WoodenWorkshop.PublicApi.Web.Core.Services;

public class WebApiAuthService
{
    private readonly IOptionsSnapshot<SecurityOptions> _securityOptions;

    private readonly IUsersClient _usersClient;

    private readonly IPasswordsClient _passwordsClient;
    
    private readonly ISessionsClient _sessionsClient;

    private int RefreshTokenTtl => _securityOptions.Value.RefreshTokenTtlMinutes;

    private const string DefaultWebSessionDescription = "Сессия в браузере.";

    public WebApiAuthService(
        IOptionsSnapshot<SecurityOptions> securityOptions,
        IUsersClient usersClient,
        IPasswordsClient passwordsClient,
        ISessionsClient sessionsClient
    )
    {
        _securityOptions = securityOptions;
        _usersClient = usersClient;
        _passwordsClient = passwordsClient;
        _sessionsClient = sessionsClient;
    }

    public async Task<AuthResult> AuthorizeAsync(AuthRequest authRequest, string ipAddress)
    {
        var user = await _usersClient.GetUserByEmailAsync(authRequest.Username);
        if (user is null)
        {
            throw new CoreLogicException("", 1);
        }
        
        var (passwordHash, _) = await _passwordsClient.HashPasswordAsync(authRequest.Password, user.PasswordSalt);
        if (!passwordHash.Equals(user.PasswordHash))
        {
            throw new CoreLogicException("", 1);
        }
        
        var associatedToken = await _sessionsClient.GetDeviceSessionAsync(user.Id, ipAddress, authRequest.DeviceName);
        if (associatedToken is not null)
        {
            return await RefreshSessionAsync(associatedToken.Token);
        }

        var session = await _sessionsClient.StartSessionAsync(new()
        {
            ExpireDate = DateTime.UtcNow.AddMinutes(RefreshTokenTtl),
            UserId = user.Id,
            IpAddress = ipAddress,
            DeviceName = authRequest.DeviceName,
            Description = DefaultWebSessionDescription,
        });
        
        return new AuthResult(
            CreateAccessToken(user.Id),
            session.ToRefreshToken()
        );
    }

    public async Task<AuthResult> RefreshSessionAsync(string refreshToken)
    {
        var refreshSessionDto = new RefreshSessionDto
        {
            ExpireDate = DateTime.UtcNow.AddMinutes(RefreshTokenTtl),
            Token = refreshToken,
        };

        var updatedSession = await _sessionsClient.RefreshSessionAsync(refreshSessionDto);
        return new AuthResult(
            CreateAccessToken(updatedSession.UserId),
            updatedSession.ToRefreshToken()
        );
    }

    public async Task TerminateSessionAsync(string refreshToken)
    {
        await _sessionsClient.TerminateSessionAsync(refreshToken);
    }

    private AccessToken CreateAccessToken(int userId)
    {
        var issueDate = DateTime.UtcNow;
        var expireDate = issueDate.AddSeconds(_securityOptions.Value.AccessTokenTtlSeconds);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityOptions.Value.JwtSecret));
        var userIdClaim = new Claim("uid", userId.ToString());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new (new []{userIdClaim}),
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenStringRepresentation = tokenHandler.CreateToken(tokenDescriptor);
        return new AccessToken
        {
            UserId = userId,
            ExpireDate = expireDate,
            Token = tokenHandler.WriteToken(tokenStringRepresentation),
        };
    }
}
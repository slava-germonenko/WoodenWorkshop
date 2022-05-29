using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

using WoodenWorkshop.Passwords.Core.Contract;
using WoodenWorkshop.Passwords.Infrastructure.Options;

namespace WoodenWorkshop.Passwords.Infrastructure.Contracts;

public class PasswordHasher : IPasswordHasher
{
    private readonly IOptionsSnapshot<SecurityOptions> _securityOptions;

    public PasswordHasher(IOptionsSnapshot<SecurityOptions> securityOptions)
    {
        _securityOptions = securityOptions;
    }

    public (string passwordHash, string salt) HashPassword(string password, string? salt = null)
    {
        var actualSalt = salt is null ? GenerateSalt() : Convert.FromBase64String(salt);
        var passwordBytes = KeyDerivation.Pbkdf2(
            password,
            actualSalt,
            KeyDerivationPrf.HMACSHA256,
            _securityOptions.Value.PasswordHashIterationCount,
            _securityOptions.Value.PasswordHashBytesLength
        );

        return (Convert.ToBase64String(passwordBytes), Convert.ToBase64String(actualSalt));
    }

    private byte[] GenerateSalt() =>  RandomNumberGenerator.GetBytes(_securityOptions.Value.PasswordSaltLength);
}
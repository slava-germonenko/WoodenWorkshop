using WoodenWorkshop.Common.Core.Exceptions;
using WoodenWorkshop.Passwords.Core.Contract;

namespace WoodenWorkshop.Passwords.Core;

public class PasswordsService
{
    private readonly IPasswordHasher _passwordHasher;

    private readonly PasswordsContext _passwordsContext;

    public PasswordsService(IPasswordHasher passwordHasher, PasswordsContext passwordsContext)
    {
        _passwordHasher = passwordHasher;
        _passwordsContext = passwordsContext;
    }

    public async Task SetUserPasswordAsync(int userId, string password)
    {
        var user = await _passwordsContext.Users.FindAsync(userId);
        if (user is null)
        {
            throw new CoreLogicException("Пользователь не найден.", 100);
        }

        var (passwordHash, passwordSalt) = GeneratePasswordHash(password);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        _passwordsContext.Update(user);
        await _passwordsContext.SaveChangesAsync();
    }

    public (string passwordHash, string passwordSalt) GeneratePasswordHash(string password, string? salt = null)
        => _passwordHasher.HashPassword(password, salt);
}
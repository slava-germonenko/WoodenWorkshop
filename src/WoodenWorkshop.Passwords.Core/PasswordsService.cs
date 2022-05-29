using WoodenWorkshop.Passwords.Core.Contract;

namespace WoodenWorkshop.Passwords.Core;

public class PasswordsService
{
    private readonly IPasswordHasher _passwordHasher;
    
    private readonly IUsersClient _usersClient;

    public PasswordsService(IPasswordHasher passwordHasher, IUsersClient usersClient)
    {
        _passwordHasher = passwordHasher;
        _usersClient = usersClient;
    }

    public async Task SetUserPasswordAsync(int userId, string password)
    {
        var user = await _usersClient.GetUserAsync(userId);
        (user.PasswordHash, user.PasswordSalt) = _passwordHasher.HashPassword(password);
        await _usersClient.UpdateUserAsync(user);
    }

    public (string passwordHash, string passwordSalt) GeneratePasswordHash(string password, string? salt = null)
        => _passwordHasher.HashPassword(password, salt);
}
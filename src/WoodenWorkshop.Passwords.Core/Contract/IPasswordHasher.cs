namespace WoodenWorkshop.Passwords.Core.Contract;

public interface IPasswordHasher
{
    public (string passwordHash, string salt) HashPassword(string password, string? salt = null);
}
namespace WoodenWorkshop.Invitations.Core.Contracts;

public interface IPasswordsClient
{
    Task SetUserPasswordAsync(int userId, string password);
}
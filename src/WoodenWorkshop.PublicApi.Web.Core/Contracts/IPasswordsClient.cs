namespace WoodenWorkshop.PublicApi.Web.Core.Contracts;

public interface IPasswordsClient
{
    public Task<(string passwordHash, string salt)> HashPasswordAsync(string password, string? salt = null);
}
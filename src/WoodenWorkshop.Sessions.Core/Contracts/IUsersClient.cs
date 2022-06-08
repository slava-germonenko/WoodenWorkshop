using WoodenWorkshop.Sessions.Core.Models;

namespace WoodenWorkshop.Sessions.Core.Contracts;

public interface IUsersClient
{
    public Task<User> GetUserAsync(int userId);
}
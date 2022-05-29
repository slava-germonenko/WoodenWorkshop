using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.PublicApi.Web.Core.Models.Users;

namespace WoodenWorkshop.PublicApi.Web.Core.Contracts;

public interface IUsersClient
{
    public Task<User> GetUserAsync(int userId);

    public Task<PagedResult<User>> GetUsersAsync(UsersFilter filter);

    public Task<User> UpdateUserAsync(User user);
}
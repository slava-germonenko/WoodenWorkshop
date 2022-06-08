using WoodenWorkshop.Common.Core.Models;
using WoodenWorkshop.PublicApi.Web.Core.Contracts;
using WoodenWorkshop.PublicApi.Web.Core.Models.Users;

namespace WoodenWorkshop.PublicApi.Web.Core.Extensions;

public static class UserClientExtensions
{
    public static async Task<User?> GetUserByEmailAsync(this IUsersClient client, string emailAddress)
    {
        var filter = new UsersFilter
        {
            EmailAddress = emailAddress,
            Count = 1,
        };
        var response = await client.GetUsersAsync(filter);
        return response.Data.FirstOrDefault();
    }

    public static async Task<PagedResult<UserViewModel>> GetUsersListForView(
        this IUsersClient client,
        UsersFilter filter
    )
    {
        var usersPagedResult = await client.GetUsersAsync(filter);
        var userViewModels = usersPagedResult.Data
            .Select(user => user.ToUserViewModel())
            .ToList();

        return new PagedResult<UserViewModel>
        {
            Count = filter.Count,
            Offset = filter.Offset,
            Total = usersPagedResult.Total,
            Data = userViewModels,
        };
    }

    public static async Task<UserViewModel> UpdateUserPersonalDataAsync(
        this IUsersClient usersClient,
        UserViewModel profile
    )
    {
        var user = await usersClient.GetUserAsync(profile.Id);
        user.FirstName = profile.FirstName;
        user.LastName = profile.LastName;
        user.EmailAddress = profile.EmailAddress;
        user.Active = profile.Active;
        return await usersClient.UpdateUserAsync(user);
    }
}
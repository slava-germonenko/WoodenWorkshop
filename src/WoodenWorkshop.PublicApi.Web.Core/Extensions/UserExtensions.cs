using WoodenWorkshop.PublicApi.Web.Core.Models.Users;

namespace WoodenWorkshop.PublicApi.Web.Core.Extensions;

public static class UserExtensions
{
    public static UserViewModel ToUserViewModel(this User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        EmailAddress = user.EmailAddress,
        Active = user.Active,
        CreatedDate = user.CreatedDate,
        UpdatedDate = user.UpdatedDate,
    };
}
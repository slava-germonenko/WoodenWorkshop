namespace WoodenWorkshop.PublicApi.Web.Core.Models.Users;

public class User : UserViewModel
{
    public string PasswordHash { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;
}
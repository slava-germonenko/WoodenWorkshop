using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Passwords.Core.Models;

public class User : BaseModel
{
    public string PasswordHash { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;
}
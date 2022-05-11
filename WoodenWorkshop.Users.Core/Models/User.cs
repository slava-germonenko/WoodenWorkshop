using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Users.Core.Models;

public class User : BaseModel
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    
    public bool Active { get; set; }

    public void CopyDetails(User user)
    {
        FirstName = user.FirstName;
        LastName = user.LastName;
        EmailAddress = user.EmailAddress;
        PasswordHash = user.PasswordHash;
        Active = user.Active;
    }
}
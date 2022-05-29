using System.ComponentModel.DataAnnotations;

using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.Users.Core.Models;

public class User : BaseModel
{
    [Required(ErrorMessage = "Имя – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Максимальная длина имени – 100 символов.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Максимальная фамилии – 100 символов.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Адрес электронной почты – обязательное поле.")]
    [StringLength(250, ErrorMessage = "Максимальная длина адреса электронной почты – 250 символов.")]
    public string EmailAddress { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;
    
    public bool Active { get; set; }

    public void CopyDetails(User user)
    {
        FirstName = user.FirstName;
        LastName = user.LastName;
        EmailAddress = user.EmailAddress;
        PasswordHash = user.PasswordHash;
        PasswordSalt = user.PasswordSalt;
        Active = user.Active;
    }
}
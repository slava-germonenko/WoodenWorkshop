using System.ComponentModel.DataAnnotations;
using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Users;

public class UserViewModel : BaseModel
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

    public bool Active { get; set; }
}
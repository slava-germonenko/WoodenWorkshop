using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Auth;

public class AuthRequest
{
    [Required(ErrorMessage = "Имя пользователя – обязательное поле.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль – обязательное поле.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Название устройства – обязательное поле.")]
    public string DeviceName { get; set; } = string.Empty;
}
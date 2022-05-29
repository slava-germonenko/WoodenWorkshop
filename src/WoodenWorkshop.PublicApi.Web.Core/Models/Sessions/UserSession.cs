using System.ComponentModel.DataAnnotations;

using WoodenWorkshop.Common.Core.Models;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

public class UserSession : BaseModel
{
    [Required(ErrorMessage = "Токен – обязательное поле.")]
    public string Token { get; set; } = string.Empty;

    public int UserId { get; set; }

    public DateTime? ExpireDate { get; set; }

    [Required(ErrorMessage = "IP Адрес – обязательное поле.")]
    public string IpAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Название устройства – обязательное поле.")]
    public string DeviceName { get; set; } = string.Empty;

    public string? Description { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

public record StartSessionDto
{
    public int UserId { get; set; }

    public DateTime? ExpireDate { get; set; }

    [Required(ErrorMessage = "IP Адрес – обязательное поле.")]
    public string IpAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Название устройства – обязательное поле.")]
    public string DeviceName { get; set; } = string.Empty;

    public string? Description { get; set; }
}
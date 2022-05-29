using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.Sessions.Core.Dtos;

public record RefreshSessionDto
{
    [Required(ErrorMessage = "Токен – обязательное поле.")]
    public string Token { get; set; } = string.Empty;

    public DateTime? ExpireDate { get; set; }
};
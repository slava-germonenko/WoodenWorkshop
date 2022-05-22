using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Sessions;

public record RefreshSessionDto
{
    [Required(ErrorMessage = "Токен – обязательное поле.")]
    public string Token { get; set; } = string.Empty;

    public DateTime? ExpireDate { get; set; }
};
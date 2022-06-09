using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.PublicApi.Web.Core.Models.Users;

public record SetPasswordModel
{
    [Required]
    public string Password { get; set; } = string.Empty;
}
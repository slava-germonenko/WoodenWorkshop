using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.Passwords.Api.Dtos;

public record SetPasswordDto
{
    public int UserId { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;
}
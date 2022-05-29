using System.ComponentModel.DataAnnotations;

namespace WoodenWorkshop.Passwords.Api.Dtos;

public record HashPasswordDto
{
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public string? Salt { get; set; }
}
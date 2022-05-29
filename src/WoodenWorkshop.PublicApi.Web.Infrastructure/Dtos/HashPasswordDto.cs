namespace WoodenWorkshop.PublicApi.Web.Infrastructure.Dtos;

public record HashPasswordDto
{
    public string PasswordHash { get; set; } = string.Empty;
    
    public string Salt { get; set; } = string.Empty;
}
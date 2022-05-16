namespace WoodenWorkshop.Passwords.Infrastructure.Options;

public record RoutingOptions
{
    public string UsersServiceUrl { get; set; } = string.Empty;
}
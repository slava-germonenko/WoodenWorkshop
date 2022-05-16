namespace WoodenWorkshop.Invitations.Infrastructure.Options;

public record RoutingOptions
{
    public string UsersServiceUrl { get; set; } = string.Empty;

    public string PasswordsServiceUrl { get; set; } = string.Empty;
}
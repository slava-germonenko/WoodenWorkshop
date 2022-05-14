namespace WoodenWorkshop.Mailing.Core.Options;

public record SmtpOptions
{
    public string Domain { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
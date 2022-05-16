namespace WoodenWorkshop.Invitations.Infrastructure.Options;

public record MailingOptions
{
    public string NoReplyEmailAddress { get; set; } = string.Empty;
    
    public string NoReplyDisplayName { get; set; } = string.Empty;
}
namespace WoodenWorkshop.Invitations.Infrastructure.Dtos;

public class MailMessageDto
{
    public string Subject { get; set; } = string.Empty;
    
    public string HtmlBody { get; set; } = string.Empty;
    
    public string FromAddress { get; set; } = string.Empty;

    public string? FromName { get; set; }
    
    public ICollection<MailRecipientDto> Recipients { get; set; } = new List<MailRecipientDto>();
}
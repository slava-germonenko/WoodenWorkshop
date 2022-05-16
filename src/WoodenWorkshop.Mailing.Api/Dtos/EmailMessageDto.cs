using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace WoodenWorkshop.Mailing.Api.Dtos;

public class EmailMessageDto
{
    public string? Subject { get; set; }
    
    [Required(ErrorMessage = "Текст письма – обязательное поле.")]
    public string HtmlBody { get; set; } = string.Empty;

    [Required(ErrorMessage = "Адрес электонно почты отправителя – обязательное поле.")]
    public string FromAddress { get; set; } = string.Empty;

    public string? FromName { get; set; }

    public ICollection<RecipientDto> Recipients { get; set; } = new List<RecipientDto>();

    public ICollection<RecipientDto>? CcRecipients { get; set; }

    public ICollection<RecipientDto>? BccRecipients { get; set; }

    public MailMessage ToMailMessage()
    {
        var message = new MailMessage
        {
            Subject = Subject,
            Body = HtmlBody,
            IsBodyHtml = true,
            From = new MailAddress(FromAddress, FromName)
        };
        foreach (var recipient in Recipients)
        {
            message.To.Add(recipient.ToMailAddress());   
        }

        if (CcRecipients is not null)
        {
            foreach (var recipient in CcRecipients)
            {
                message.CC.Add(recipient.ToMailAddress());
            }
        }
        
        if (BccRecipients is not null)
        {
            foreach (var recipient in BccRecipients)
            {
                message.Bcc.Add(recipient.ToMailAddress());
            }
        }

        return message;
    }
}
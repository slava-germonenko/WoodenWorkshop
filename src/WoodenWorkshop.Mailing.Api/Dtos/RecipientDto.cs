using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace WoodenWorkshop.Mailing.Api.Dtos;

public record RecipientDto
{
    public string? DisplayName { get; set; }

    [Required(ErrorMessage = "Адерс электронной почты – обязательное поле.")]
    public string EmailAddress { get; set; } = string.Empty;

    public MailAddress ToMailAddress() => new(EmailAddress, DisplayName);
}
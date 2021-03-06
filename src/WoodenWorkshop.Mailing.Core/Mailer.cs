using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Options;

using WoodenWorkshop.Mailing.Core.Options;

namespace WoodenWorkshop.Mailing.Core;

public class Mailer
{
    private readonly IOptionsSnapshot<SmtpOptions> _smtpOptions;

    private string SmtpDomain => _smtpOptions.Value.Domain;

    private int SmtpPort => _smtpOptions.Value.Port;

    private ICredentialsByHost SmtpCredentials => new NetworkCredential(
        _smtpOptions.Value.Username,
        _smtpOptions.Value.Password
    );

    public Mailer(IOptionsSnapshot<SmtpOptions> smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }

    public async Task SendEmail(MailMessage message)
    {
        var smtpClient = new SmtpClient(SmtpDomain, SmtpPort)
        {
            EnableSsl = true,
            Credentials = SmtpCredentials
        };

        await smtpClient.SendMailAsync(message);
    }
}
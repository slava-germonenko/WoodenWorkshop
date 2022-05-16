using System.Text.Json;

using Microsoft.Extensions.Options;
using RabbitMQ.Client;

using WoodenWorkshop.Invitations.Core.Contracts;
using WoodenWorkshop.Invitations.Core.Dtos;
using WoodenWorkshop.Invitations.Infrastructure.Dtos;
using WoodenWorkshop.Invitations.Infrastructure.Options;

namespace WoodenWorkshop.Invitations.Infrastructure.Contracts;

public class MessageQueueMailingClient : IMailingClient
{
    private const string EmailQueueName = "email";

    private readonly IConnectionFactory _connectionFactory;

    private readonly IOptionsSnapshot<MailingOptions> _mailingOptions;

    public MessageQueueMailingClient(
        IConnectionFactory connectionFactory,
        IOptionsSnapshot<MailingOptions> mailingOptions
    )
    {
        _connectionFactory = connectionFactory;
        _mailingOptions = mailingOptions;
    }

    public Task QueueInvitationEmailAsync(InvitationMailDto invitation)
    {
        var mailDto = BuildQueueMessage(invitation);
        QueueMail(mailDto);
        return Task.CompletedTask;
    }

    private MailMessageDto BuildQueueMessage(InvitationMailDto invitation) => new()
    {
        Recipients = new List<MailRecipientDto>
        {
            new ()
            {
                EmailAddress = invitation.EmailAddress
            }    
        },
        Subject = invitation.Subject,
        HtmlBody = invitation.Body,
        FromAddress = _mailingOptions.Value.NoReplyEmailAddress,
        FromName = _mailingOptions.Value.NoReplyDisplayName
    };

    private void QueueMail(MailMessageDto mailMessage)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(EmailQueueName, true, false, false);
        channel.BasicPublish(
            string.Empty,
            EmailQueueName,
            body: JsonSerializer.SerializeToUtf8Bytes(mailMessage)
        );
    }
}
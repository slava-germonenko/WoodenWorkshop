using MediatR;
using WoodenWorkshop.Mailing.Api.Dtos;

namespace WoodenWorkshop.Mailing.Api.Commands;

public class SendEmailCommand : IRequest<bool>
{
    public EmailMessageDto Message { get; }

    public SendEmailCommand(EmailMessageDto message)
    {
        Message = message;
    }
}
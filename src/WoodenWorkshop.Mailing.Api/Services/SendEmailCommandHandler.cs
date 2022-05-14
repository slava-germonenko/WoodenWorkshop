using MediatR;

using WoodenWorkshop.Mailing.Api.Commands;
using WoodenWorkshop.Mailing.Core;

namespace WoodenWorkshop.Mailing.Api.Services;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
{
    private readonly Mailer _mailer;

    public SendEmailCommandHandler(Mailer mailer)
    {
        _mailer = mailer;
    }

    public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        await _mailer.SendEmail(request.Message.ToMailMessage());
        return true;
    }
}
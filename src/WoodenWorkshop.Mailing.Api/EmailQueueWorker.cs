using System.Text;
using System.Text.Json;

using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using WoodenWorkshop.Mailing.Api.Commands;
using WoodenWorkshop.Mailing.Api.Dtos;

namespace WoodenWorkshop.Mailing.Api;

public class EmailQueueWorker : BackgroundService
{
    private const string EmailQueueName = "email";
    
    private readonly EventingBasicConsumer _consumer;

    private readonly IModel _channel;

    private readonly IMediator _mediator;

    public EmailQueueWorker(IMediator mediator, IConnectionFactory connectionFactory)
    {
        _mediator = mediator;
        _channel = connectionFactory.CreateConnection().CreateModel();
        _channel.QueueDeclare(EmailQueueName, true, false, false);
        _consumer = new EventingBasicConsumer(_channel);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Received += (_, eventArgs) =>
        {
            var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var emailMessage = JsonSerializer.Deserialize<EmailMessageDto>(content);
            if (emailMessage is not null)
            {
                _mediator.Publish(new SendEmailCommand(emailMessage));
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            else
            {
                _channel.BasicReject(eventArgs.DeliveryTag, false);
            }
        };

        _channel.BasicConsume(EmailQueueName, false, _consumer);
        return Task.CompletedTask;
    }
}
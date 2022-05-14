using System.Text;
using System.Text.Json;

using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using WoodenWorkshop.Common.Utils.Actions;
using WoodenWorkshop.Mailing.Api.Commands;
using WoodenWorkshop.Mailing.Api.Dtos;

namespace WoodenWorkshop.Mailing.Api;

public class EmailQueueWorker : BackgroundService
{
    private const string EmailQueueName = "email";
    
    private readonly RetryActionRunner _connectionRetryActionRunner = new(TimeSpan.FromSeconds(3), 2);

    private readonly IConnectionFactory _connectionFactory;

    private readonly IMediator _mediator;
    
    private IModel? _channel;
    
    private EventingBasicConsumer? _consumer;

    public EmailQueueWorker(IMediator mediator, IConnectionFactory connectionFactory)
    {
        _mediator = mediator;
        _connectionFactory = connectionFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SetupMailQueueAsync();
        _consumer!.Received += (_, eventArgs) =>
        {
            var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var emailMessage = JsonSerializer.Deserialize<EmailMessageDto>(content);
            if (emailMessage is not null)
            {
                _mediator.Publish(new SendEmailCommand(emailMessage));
                _channel!.BasicAck(eventArgs.DeliveryTag, false);
            }
            else
            {
                _channel!.BasicReject(eventArgs.DeliveryTag, false);
            }
        };

        _channel.BasicConsume(EmailQueueName, false, _consumer);
    }

    private async Task SetupMailQueueAsync()
    {
        _channel = await _connectionRetryActionRunner.RunWithRetryAsync(
            () => _connectionFactory.CreateConnection().CreateModel()
        );
        _channel.QueueDeclare(EmailQueueName, true, false, false);
        _consumer = new EventingBasicConsumer(_channel);
    }
}
using System.Text;
using System.Text.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using WoodenWorkshop.Common.Utils.Actions;
using WoodenWorkshop.Mailing.Api.Dtos;
using WoodenWorkshop.Mailing.Core;

namespace WoodenWorkshop.Mailing.Api;

public class EmailQueueWorker : BackgroundService
{
    private const string EmailQueueName = "email";
    
    private readonly RetryActionRunner _connectionRetryActionRunner = new(TimeSpan.FromSeconds(5), 3);

    private readonly IConnectionFactory _connectionFactory;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IModel? _channel;
    
    private EventingBasicConsumer? _consumer;

    public EmailQueueWorker(IServiceScopeFactory serviceScopeFactory, IConnectionFactory connectionFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _connectionFactory = connectionFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SetupMailQueueAsync();
        _consumer!.Received += async (_, eventArgs) =>
        {
            var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var emailMessage = JsonSerializer.Deserialize<EmailMessageDto>(content);
            if (emailMessage is not null)
            {
                await SendEmailSafe(emailMessage);
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

    private async Task SendEmailSafe(EmailMessageDto emailMessageDto)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mailer = scope.ServiceProvider.GetRequiredService<Mailer>();
            await mailer.SendEmail(emailMessageDto.ToMailMessage());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Will need to add logging here in the future
        }
    }
}
using System.Text;
using CalendarNotifier.NotificationWorker.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CalendarNotifier.NotificationWorker;
public class Worker(IOptions<TelegramSettings> options) : BackgroundService
{
    private readonly TelegramSettings _settings = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var telegram = new TelegramService(_settings.BotToken);
        await using var connection = await RabbitMqConnection.CreateAsync();
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        try
        {
            await channel.QueueDeclareAsync(
                queue: "calendar-events",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                Console.WriteLine("Mensagem recebida da fila.");
                
                await telegram.SendMessage(_settings.ChatId, message);
                
                Console.WriteLine("Mensagem enviada ao Telegram.");
                
                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false,
                    cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        };

        await channel.BasicConsumeAsync(
            queue: "calendar-events",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);
        
        Console.WriteLine("Aguardando mensagens...");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
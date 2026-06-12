using System.Text;
using CalendarNotifier.Messaging;
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
        
        await using var connection = 
            await RabbitMqConnection.CreateAsync();
        
        await using var channel = 
            await  connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await RabbitMqTopology.DeclareAsync(channel, stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        
        var maxRetries = 3;
        
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var processed = false;
            var retryCount = 0;

            while (!processed && retryCount < maxRetries)
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                
                    Console.WriteLine(
                        $"Tentativa {retryCount + 1}");
                
                    await telegram.SendMessage(_settings.ChatId, message);
                
                    Console.WriteLine("Mensagem enviada ao Telegram.");
                
                    // SIMULA FALHA
                    // throw new Exception("Falha proposital");
                
                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: stoppingToken);
                    
                    Console.WriteLine("Mensagem processada.");
                    
                    processed = true;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    
                    Console.WriteLine(
                        "Mensagem enviada para DLQ.");
                    
                    await channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: false,
                        cancellationToken: stoppingToken);
                }
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
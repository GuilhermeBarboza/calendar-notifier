using System.Text;
using System.Text.Json;
using CalendarNotifier.Messaging.RabbitMq;
using CalendarNotifier.Worker.Formatting;
using CalendarNotifier.Worker.Google;
using CalendarNotifier.Worker.Mapping;


namespace CalendarNotifier.Worker;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Consultando agenda...");

                var publisher = new CalendarNotificationPublisher(
                    new GoogleCalendarService());
                await publisher.PublishAsync(stoppingToken);
                
                Console.WriteLine("Mensagem publicada na fila.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        
    }
}

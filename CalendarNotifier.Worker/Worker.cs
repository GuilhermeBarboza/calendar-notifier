using System.Text;
using System.Text.Json;
using CalendarNotifier.Messaging.RabbitMq;
using CalendarNotifier.Worker.Formatting;
using CalendarNotifier.Worker.Google;
using CalendarNotifier.Worker.Mapping;
using CalendarNotifier.Worker.Scheduling;


namespace CalendarNotifier.Worker;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            var delay = DailyScheduler.GetDelay(
                DateTime.Now,
                new TimeOnly(8, 0));
            
            Console.WriteLine(
                $"Próxima execução em {delay.ToString()}");
            
            await Task.Delay(delay, stoppingToken);
            
            Console.WriteLine("Consultando agenda...");
            
            var publisher = new CalendarNotificationPublisher(
                new GoogleCalendarService());
            await publisher.PublishAsync(stoppingToken);
            
            Console.WriteLine("Mensagem publicada na fila.");
        }
        
    }
}

using CalendarNotifier.Worker.Configurations;
using CalendarNotifier.Worker.Google;
using CalendarNotifier.Worker.Scheduling;
using Microsoft.Extensions.Options;


namespace CalendarNotifier.Worker;

public class Worker(
    ILogger<Worker> logger,
    IOptions<NotificationSettings> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            var delay = DailyScheduler.GetDelay(
                DateTime.Now,
                TimeOnly.Parse(options.Value.ExecutionTime));
            
            Console.WriteLine(
                $"Próxima execução em {delay.ToString()}");
            
            await Task.Delay(delay, stoppingToken);
            
            Console.WriteLine("Consultando agenda...");
            
            var publisher = new CalendarNotificationPublisher(
                new GoogleCalendarService());
            await publisher.PublishAsync(options.Value.DaysAhead, stoppingToken);
            
            Console.WriteLine("Mensagem publicada na fila.");
        }
        
    }
}

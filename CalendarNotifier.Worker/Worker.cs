using CalendarNotifier.Worker.Formatting;
using CalendarNotifier.Worker.Google;

namespace CalendarNotifier.Worker;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var calendar = new GoogleCalendarService();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Consultando agenda...");

                var events = await calendar.GetNext30DaysEvents();
                var message = MessageFormatter.Format(events);
                
                var sharedPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "shared");

                sharedPath = Path.GetFullPath(sharedPath);
                Directory.CreateDirectory(sharedPath);

                var fileName = Path.Combine(
                    sharedPath,
                    $"message-{DateTime.Now:yyyyMMddHHmmss}.txt");
                
                await File.WriteAllTextAsync(fileName, message);
                
                Console.WriteLine($"Arquivo gerado: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        
    }
}

using CalendarNotifier.Worker.Formatting;
using CalendarNotifier.Worker.Google;

namespace CalendarNotifier.Worker;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var calendar = new GoogleCalendarService();
        var telegram = new TelegramService("8700195733:AAH0zjZUSRDyUPG8hQiM5lXgzE2qUgAyS3Q");
        long chatId = -5151219139;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Consultando agenda...");

                var events = await calendar.GetNext30DaysEvents();
                var message = MessageFormatter.Format(events);

                Console.WriteLine("Enviando mensagem para Telegram...");

                await telegram.SendMessage(chatId, message);

                Console.WriteLine("Mensagem enviada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        
    }
}

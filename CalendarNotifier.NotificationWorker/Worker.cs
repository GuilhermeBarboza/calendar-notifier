using CalendarNotifier.NotificationWorker.Configurations;
using Microsoft.Extensions.Options;

namespace CalendarNotifier.NotificationWorker;
public class Worker(IOptions<TelegramSettings> options) : BackgroundService
{
    private readonly TelegramSettings _settings = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var telegram = new TelegramService(_settings.BotToken);
        long chatId = _settings.ChatId;

        while (!stoppingToken.IsCancellationRequested)
        {
            var sharedPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "shared");

            sharedPath = Path.GetFullPath(sharedPath);

            Directory.CreateDirectory(sharedPath);

            var files = Directory.GetFiles(sharedPath, "*.txt");

            foreach (var file in files)
            {
                try
                {
                    var message = await File.ReadAllTextAsync(file);

                    await telegram.SendMessage(chatId, message);

                    File.Delete(file);

                    Console.WriteLine($"Arquivo processado: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
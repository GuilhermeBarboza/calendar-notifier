namespace CalendarNotifier.NotificationWorker;
public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var telegram = new TelegramService("8700195733:AAH0zjZUSRDyUPG8hQiM5lXgzE2qUgAyS3Q");

        long chatId = -5151219139;

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
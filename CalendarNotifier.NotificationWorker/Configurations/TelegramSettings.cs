namespace CalendarNotifier.NotificationWorker.Configurations;

public class TelegramSettings
{
    public string BotToken { get; set; } = string.Empty;

    public long ChatId { get; set; }
}
using Telegram.Bot;

public class TelegramService(string token)
{
    private readonly ITelegramBotClient _bot = new TelegramBotClient(token);

    public async Task SendMessage(long chatId, string message)
    {
        await _bot.SendMessage(chatId, message);
    }
}
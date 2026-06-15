using System.Text;
using CalendarNotifier.Messaging.Contracts;

namespace CalendarNotifier.NotificationWorker.Formatters;

public static class TelegramMessageFormatter
{
    public static string Format(CalendarNotification notification)
    {
        if (notification.Events.Count == 0)
            return "📭 Nenhum evento nos próximos 30 dias.";
        
        var sb = new StringBuilder();
        
        sb.AppendLine("📅 Eventos dos próximos 30 dias");
        sb.AppendLine();

        foreach (var ev in notification.Events)
        {
            sb.AppendLine($"• {ev.Start:dd/MM/yyyy HH:mm}");
            sb.AppendLine(ev.Title);
            
            if (!string.IsNullOrWhiteSpace(ev.Location))
                sb.AppendLine($"  📍 {ev.Location}");
            
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}
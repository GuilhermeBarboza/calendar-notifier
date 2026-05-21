using System.Text;
using Google.Apis.Calendar.v3.Data;

namespace CalendarNotifier.Worker.Formatting;

public static class MessageFormatter
{
    public static string Format(IList<Event> events)
    {
        if (!events.Any())
            return "📭 Nenhum evento nos próximos 30 dias.";

        var sb = new StringBuilder();

        sb.AppendLine("📅 Eventos dos próximos 30 dias");
        sb.AppendLine();

        foreach (var ev in events)
        {
            var start = 
                ev.Start.DateTimeDateTimeOffset ??
                DateTime.Parse(ev.Start.Date);
            
            sb.AppendLine($"• {start:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"  {ev.Summary}");
            
            if (!string.IsNullOrWhiteSpace(ev.Location))
                sb.AppendLine($"  📍 {ev.Location}");
            
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}
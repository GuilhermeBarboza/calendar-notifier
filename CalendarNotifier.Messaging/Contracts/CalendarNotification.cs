namespace CalendarNotifier.Messaging.Contracts;

public class CalendarNotification
{
    public DateTime GeneratedAt { get; set; }
    public List<CalendarEventItem> Events { get; set; } = [];
}
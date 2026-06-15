namespace CalendarNotifier.Messaging.Contracts;

public class CalendarEventItem
{
    public string Title  { get; set; } = string.Empty;
    public string Location  { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
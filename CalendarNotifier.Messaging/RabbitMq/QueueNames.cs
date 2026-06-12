namespace CalendarNotifier.Messaging.RabbitMq;

public static class QueueNames
{
    public const string CALENDAR_EVENTS = "calendar-events";
    public const string CALENDAR_EVENTS_RETRY = "calendar-events-retry";
    public const string CALENDAR_EVENTS_DLQ = "calendar-events-dlq";
}
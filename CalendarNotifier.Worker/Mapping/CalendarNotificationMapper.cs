using CalendarNotifier.Messaging.Contracts;
using Google.Apis.Calendar.v3.Data;

namespace CalendarNotifier.Worker.Mapping;

public static class CalendarNotificationMapper
{
    public static Messaging.Contracts.CalendarNotification Map(
        IList<Event> events)
    {
        return new Messaging.Contracts.CalendarNotification
        {
            GeneratedAt = DateTime.UtcNow,
            Events = events.Select(e =>
                new CalendarEventItem
                {
                    Title = e.Summary ??
                            "Sem título",
                    Start = e.Start.DateTimeDateTimeOffset?.DateTime ??
                            DateTime.Parse(e.Start.Date),
                    End = e.End.DateTimeDateTimeOffset?.DateTime ??
                          DateTime.Parse(e.End.Date),
                    Location = e.Location,
                }).ToList()
        };
    }
}
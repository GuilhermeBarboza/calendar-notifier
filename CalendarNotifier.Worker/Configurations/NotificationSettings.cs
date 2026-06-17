namespace CalendarNotifier.Worker.Configurations;

public class NotificationSettings
{
    public string ExecutionTime { get; set; } = "08:00";
    public int DaysAhead { get; set; } = 30;
}
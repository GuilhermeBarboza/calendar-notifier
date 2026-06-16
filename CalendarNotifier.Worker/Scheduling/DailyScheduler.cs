namespace CalendarNotifier.Worker.Scheduling;

public static class DailyScheduler
{
    public static TimeSpan GetDelay(
        DateTime now,
        TimeOnly executionTime)
    {
        var nextExecution = now.Date.Add(executionTime.ToTimeSpan());

        if (nextExecution <= now)
            nextExecution = nextExecution.AddDays(1);
        
        return nextExecution - now;
    }
}
using System.Text;
using System.Text.Json;
using CalendarNotifier.Messaging.RabbitMq;
using CalendarNotifier.Worker.Google;
using CalendarNotifier.Worker.Mapping;
using RabbitMQ.Client;

namespace CalendarNotifier.Worker;

public class CalendarNotificationPublisher (GoogleCalendarService _calendarService)
{
    public async Task PublishAsync(
        int daysAhead, 
        CancellationToken stoppingToken)
    {
        await using var connection = 
            await RabbitMqConnection.CreateAsync();
        
        await using var channel = 
            await  connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await RabbitMqTopology.DeclareAsync(channel, stoppingToken);
        
        var events = await _calendarService.GetEventsAsync(daysAhead);
        var notification = CalendarNotificationMapper.Map(events);
        var json = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(json);
        
        await channel.BasicPublishAsync<BasicProperties>(
            exchange: "",
            routingKey: "calendar-events",
            mandatory: false,
            basicProperties: new BasicProperties(),
            body: body,
            cancellationToken: stoppingToken);
    }
}
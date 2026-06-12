using RabbitMQ.Client;

namespace CalendarNotifier.Messaging;

public static class RabbitMqTopology
{
    public static async Task DeclareAsync(
        IChannel channel,
        CancellationToken cancellationToken)
    {
        var retryArgs = new Dictionary<string, object?>
        {
            { "x-message-ttl", 1500 },
            { "x-dead-letter-exchange", "" },
            { "x-dead-letter-routing-key", QueueNames.CALENDAR_EVENTS }
        };

        await channel.QueueDeclareAsync(
            queue: QueueNames.CALENDAR_EVENTS_RETRY,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: retryArgs,
            cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queue: QueueNames.CALENDAR_EVENTS_DLQ,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);
        
        var mainArgs = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", "" },
            { "x-dead-letter-routing-key", QueueNames.CALENDAR_EVENTS_RETRY }
        };

        await channel.QueueDeclareAsync(
            queue: QueueNames.CALENDAR_EVENTS,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: mainArgs,
            cancellationToken: cancellationToken);
    }
    
    
}
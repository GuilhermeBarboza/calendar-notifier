using System.Text;
using CalendarNotifier.Messaging;
using CalendarNotifier.Worker.Formatting;
using CalendarNotifier.Worker.Google;
using RabbitMQ.Client;

namespace CalendarNotifier.Worker;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var calendar = new GoogleCalendarService();
        await using var connection = 
            await RabbitMqConnection.CreateAsync();
        
        await using var channel = 
            await  connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await RabbitMqTopology.DeclareAsync(channel, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Consultando agenda...");

                var events = await calendar.GetNext30DaysEvents();
                var message = MessageFormatter.Format(events);
                var body = Encoding.UTF8.GetBytes(message);

                
                await channel.BasicPublishAsync<BasicProperties>(
                    exchange: "",
                    routingKey: "calendar-events",
                    mandatory: false,
                    basicProperties: new BasicProperties(),
                    body: body,
                    cancellationToken: stoppingToken);
                
                Console.WriteLine("Mensagem publicada na fila.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        
    }
}

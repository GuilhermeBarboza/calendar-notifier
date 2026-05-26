using RabbitMQ.Client;

namespace CalendarNotifier.Worker.Configurations;

public static class RabbitMqConnection
{
    public static async Task<IConnection> CreateAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };

        return await factory.CreateConnectionAsync();
    }
}
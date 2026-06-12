using RabbitMQ.Client;

namespace CalendarNotifier.Messaging;

public static class RabbitMqConnection
{
    public static async Task<IConnection> CreateAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = RabbitMqSettings.Host,
            UserName = RabbitMqSettings.Username,
            Password = RabbitMqSettings.Password,
        };

        return await factory.CreateConnectionAsync();
    }
}
using CalendarNotifier.NotificationWorker;
using CalendarNotifier.NotificationWorker.Configurations;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<TelegramSettings>(
    builder.Configuration.GetSection("Telegram"));

var host = builder.Build();
host.Run();

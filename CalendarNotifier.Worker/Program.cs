using CalendarNotifier.Worker;
using CalendarNotifier.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<NotificationSettings>(
    builder.Configuration.GetSection("Notification"));

var host = builder.Build();
host.Run();

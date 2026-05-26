using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace CalendarNotifier.Worker.Google;

public class GoogleCalendarService
{
    public async Task<IList<Event>> GetNext30DaysEvents()
    {
        var credencial = await GoogleAuth.GetCredentialAsync();

        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credencial,
            ApplicationName = "CalendarNotifier",
        });
        
        var request = service.Events.List("primary");
        request.TimeMinDateTimeOffset = DateTime.Now;
        request.TimeMaxDateTimeOffset = DateTime.Now.AddDays(30);
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        
        var events = await request.ExecuteAsync();
        return events.Items;
        
    }
}
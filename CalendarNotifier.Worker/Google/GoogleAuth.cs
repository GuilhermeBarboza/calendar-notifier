using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;

namespace CalendarNotifier.Worker.Google;

public static class GoogleAuth
{
    public static async Task<UserCredential> GetCredentialAsync()
    {
        await using var stream = new FileStream("client_secret.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read);
        
        var credPath = "token.json";

        return await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[] { CalendarService.Scope.CalendarReadonly },
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)
        );
    }
}
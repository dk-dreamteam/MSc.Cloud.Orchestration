using System.Net.Http.Json;
using static MSc.Cloud.Orchestration.Common.NamesValues.HttpClientNames;

namespace MSc.Cloud.Orchestration.Common.Services;

internal class SendEmailService(
    IHttpClientFactory httpClientFactory,
    string sendEmailFunctionEndpoint) : ISendEmailService
{
    public async Task SendReservationConfirmationEmailAsync(string emailAddress, string venueName, DateTime dateTime, CancellationToken cancellationToken)
    {
        // send email with supabase function.
        var supabasePayload = new
        {
            emailAddress,
            venueName,
            dateTime,
        };

        var supabaseClient = httpClientFactory.CreateClient(SupabaseClient);

        // call supabase serverless function to send email.
        var supabaseHttpRes = await supabaseClient.PostAsJsonAsync(sendEmailFunctionEndpoint, supabasePayload, cancellationToken);

        // log to console status.
        Console.WriteLine($"Sent Reservation Confirmation Email with emailAddress:{emailAddress},venueName:{venueName}, dateTime:{dateTime} and the http response code is:{supabaseHttpRes.StatusCode}.");
    }
}

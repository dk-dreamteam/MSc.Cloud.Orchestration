using System.Net.Http.Json;

namespace MSc.Cloud.Orchestration.Common.Services;

internal class SendEmailService(HttpClient httpClient, string sendEmailFunctionEndpoint) : ISendEmailService
{
    public async Task SendReservationConfirmationEmailAsync(string emailAddress, string venueName, DateTime dateTime, CancellationToken cancellationToken)
    {
        // setup payload. 
        var payload = new
        {
            emailAddress,
            venueName,
            dateTime
        };

        // call supabase serverless function to send email.
        var res = await httpClient.PostAsJsonAsync(sendEmailFunctionEndpoint, payload, cancellationToken);

        // instead of logging to txt files, write to console.
        Console.WriteLine(
            $"Send Reservation Confirmation Email with emailAddress:{0}, venueName:{1}, dateTime:{2}, and the http response code is:{3} ",
            emailAddress,
            venueName,
            dateTime,
            res.StatusCode);
    }
}

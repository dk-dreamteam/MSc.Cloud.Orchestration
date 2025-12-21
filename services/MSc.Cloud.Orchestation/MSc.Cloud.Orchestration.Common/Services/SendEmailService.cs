using System.Net.Http.Json;
using static MSc.Cloud.Orchestration.Common.NamesValues.HttpClientNames;

namespace MSc.Cloud.Orchestration.Common.Services;

internal class SendEmailService(
    IHttpClientFactory httpClientFactory,
    string getGifEndpoint,
    string sendEmailFunctionEndpoint) : ISendEmailService
{
    public async Task SendReservationConfirmationEmailAsync(string emailAddress, string venueName, DateTime dateTime, CancellationToken cancellationToken)
    {
        // get the gif from countdownmail.com.
        var gifClient = httpClientFactory.CreateClient(GifClient);

        var gifPayload = new
        {
            skin_id = 4,
            time_end = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            time_zone = "Europe/Athens",
            font_family = "Roboto-Bold",
            color_primary = "080808",
            color_text = "F7FAFA",
            color_bg = "FFFFFF",
            transparent = "1"
        };
        var gifHttpRes = await gifClient.PostAsJsonAsync(getGifEndpoint, gifPayload, cancellationToken);
        var gifResponse = await gifHttpRes.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken);
        
        // log to console status.
        Console.WriteLine(
           $"Got GIF from CountdownMail with code {gifHttpRes.StatusCode} and url:{gifResponse?.Message.Src}");

        // if we didn't get a gif, do not send email.
        if (!gifHttpRes.IsSuccessStatusCode || cancellationToken.IsCancellationRequested) return;

        // send email with supabase function.
        var supabasePayload = new
        {
            emailAddress,
            venueName,
            dateTime,
            gifCode = gifResponse!.Message.Code
        };

        var supabaseClient = httpClientFactory.CreateClient(SupabaseClient);

        // call supabase serverless function to send email.
        var supabaseHttpRes = await supabaseClient.PostAsJsonAsync(sendEmailFunctionEndpoint, supabasePayload, cancellationToken);

        // log to console status.
        Console.WriteLine($"Sent Reservation Confirmation Email with emailAddress:{emailAddress},venueName:{venueName}, dateTime:{dateTime}, gifCode:{gifResponse!.Message.Code} and the http response code is:{supabaseHttpRes.StatusCode}.");
    }
}

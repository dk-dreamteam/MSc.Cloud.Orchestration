

namespace MSc.Cloud.Orchestration.Common.Services;

public interface ISendEmailService
{
    Task SendReservationConfirmationEmailAsync(string emailAddress, string venueName, DateTime dateTime, CancellationToken cancellationToken);
}
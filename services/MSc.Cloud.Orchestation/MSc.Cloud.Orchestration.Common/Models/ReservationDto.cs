namespace MSc.Cloud.Orchestration.Common.Models;

public record ReservationDto(
    int ReservationId,
    string FullName,
    int NumTickets,
    string EmailAddress,
    DateTime CreatedOn,
    int EventId,
    string EventName,
    DateTime StartsAt
);

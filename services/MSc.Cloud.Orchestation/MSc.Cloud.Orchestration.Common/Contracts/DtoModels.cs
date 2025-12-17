namespace MSc.Cloud.Orchestration.Common.Contracts;

public record EventDto(
    int Id,
    string Name,
    string Description,
    DateTime StartsAt,
    DateTime CreatedOn,
    string ImgUrl,
    bool IsDeleted
);

public record ReservationDto(
    int ReservationId,
    string FullName,
    int NumTickets,
    DateTime CreatedOn,
    int EventId,
    string EventName,
    DateTime StartsAt
);

public record CreateEventRequest(
    string Name,
    string Description,
    DateTime StartsAt,
    string ImgUrl
);

public record CreateReservationRequest(
    int EventId,
    string FullName,
    int NumTickets
);

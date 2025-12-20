using System.ComponentModel.DataAnnotations;

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
    string EmailAddress,
    DateTime CreatedOn,
    int EventId,
    string EventName,
    DateTime StartsAt
);

public record CreateEventRequest(
    [Required]
    string Name,

    [Required]
    string Description,

    [Required]
    DateTime StartsAt,

    [Required]
    [Url]
    string ImgUrl
);

public record CreateReservationRequest(
    [Required]
    int EventId,

    [Required]
    string FullName,

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be greater than zero.")]
    int NumTickets,

    [Required]
    [EmailAddress]
    string EmailAddress
);

using System.ComponentModel.DataAnnotations;

namespace MSc.Cloud.Orchestration.Common.Models;

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

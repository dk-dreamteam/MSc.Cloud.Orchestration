using System.ComponentModel.DataAnnotations;

namespace MSc.Cloud.Orchestration.Common.Contracts;

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

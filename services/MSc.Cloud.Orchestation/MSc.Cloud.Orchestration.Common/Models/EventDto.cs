namespace MSc.Cloud.Orchestration.Common.Models;

public record EventDto(
    int Id,
    string Name,
    string Description,
    DateTime StartsAt,
    DateTime CreatedOn,
    string ImgUrl,
    bool IsDeleted
);

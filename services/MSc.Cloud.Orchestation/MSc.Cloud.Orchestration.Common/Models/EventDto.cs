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

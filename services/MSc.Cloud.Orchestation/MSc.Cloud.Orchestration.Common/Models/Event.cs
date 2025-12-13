namespace MSc.Cloud.Orchestration.Common.Models;

public class Event
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartsAt { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? ImgUrl { get; set; }

    public bool IsDeleted { get; set; }
}

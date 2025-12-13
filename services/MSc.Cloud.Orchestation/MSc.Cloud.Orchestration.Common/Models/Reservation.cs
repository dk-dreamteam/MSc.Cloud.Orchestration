namespace MSc.Cloud.Orchestration.Common.Models;

public class Reservation
{
    public required int Id { get; set; }
    public required int EventId { get; set; }
    public required string FullName { get; set; }
}

namespace MSc.Cloud.Orchestration.EventsService.Models;

public class Reservation
{
    public required int Id { get; set; }
    public required int EventId { get; set; }
    public required string FullName { get; set; }
}

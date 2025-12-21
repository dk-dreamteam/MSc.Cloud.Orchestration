using Microsoft.AspNetCore.Mvc;
using MSc.Cloud.Orchestration.Common.Models;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using MSc.Cloud.Orchestration.Common.Services;

namespace MSc.Cloud.Orchestration.ReservationsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController(
    IEventRepository eventsRepository,
    IReservationRepository reservationsRepository,
    ISendEmailService sendEmailService) : ControllerBase
{
    /// <summary>
    /// Create new Reservation on given Event.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request, CancellationToken cancellationToken)
    {
        // check if event exists.
        var ev = await eventsRepository.GetByIdAsync(request.EventId);
        if (ev is null)
            return NotFound($"Event with id {request.EventId} not found.");

        // create reservation and get new reservation id.
        var id = await reservationsRepository.CreateAsync(request);

        // after successful reservation, send confirmation email and don't await for result.
        await sendEmailService.SendReservationConfirmationEmailAsync(request.EmailAddress, ev.Name, ev.StartsAt, cancellationToken);

        // return 201 Created with new reservation id.
        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    /// <summary>
    /// Get a Reservation by Id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await reservationsRepository.GetByIdAsync(id);

        if (reservation is null)
            return NotFound();

        return Ok(reservation);
    }

    /// <summary>
    /// Get Reservations.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var reservations = await reservationsRepository.ListAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// Delete a Reservation by Id.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await reservationsRepository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
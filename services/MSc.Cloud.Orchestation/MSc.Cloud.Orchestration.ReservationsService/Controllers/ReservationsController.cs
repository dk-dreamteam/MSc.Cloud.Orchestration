using Microsoft.AspNetCore.Mvc;
using MSc.Cloud.Orchestration.Common.Models;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;

namespace MSc.Cloud.Orchestration.ReservationsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController(IReservationRepository repository) : ControllerBase
{
    /// <summary>
    /// Create new Reservation on given Event.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        var id = await repository.CreateAsync(request);

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
        var reservation = await repository.GetByIdAsync(id);

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
        var reservations = await repository.ListAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// Delete a Reservation by Id.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
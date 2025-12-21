using Microsoft.AspNetCore.Mvc;
using MSc.Cloud.Orchestration.Common.Models;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;

namespace MSc.Cloud.Orchestration.EventsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EventsController(IEventRepository repository) : ControllerBase
{
    /// <summary>
    /// Create new event.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        if (request.StartsAt <= DateTime.UtcNow)
            return BadRequest("StartsAt must be in the future.");

        var id = await repository.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    /// <summary>
    /// Get an event by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await repository.GetByIdAsync(id);

        if (ev is null)
            return NotFound();

        return Ok(ev);
    }

    /// <summary>
    /// Get all events.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var events = await repository.ListAsync();
        return Ok(events);
    }

    /// <summary>
    /// Mark an event as deleted.
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
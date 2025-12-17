using Microsoft.AspNetCore.Mvc;
using MSc.Cloud.Orchestration.Common.Contracts;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;

namespace MSc.Cloud.Orchestration.EventsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EventsController(IEventRepository repository) : ControllerBase
{
    // POST: api/events
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        if (request.StartsAt <= DateTime.UtcNow)
            return BadRequest("StartsAt must be in the future.");

        var id = await repository.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    // GET: api/events/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await repository.GetByIdAsync(id);

        if (ev is null)
            return NotFound();

        return Ok(ev);
    }

    // GET: api/events
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var events = await repository.ListAsync();
        return Ok(events);
    }

    // DELETE: api/events/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
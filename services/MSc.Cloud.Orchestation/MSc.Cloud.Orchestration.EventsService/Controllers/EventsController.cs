using Dapper;
using Microsoft.AspNetCore.Mvc;
using MSc.Cloud.Orchestration.Common;
using MSc.Cloud.Orchestration.Common.Models;
using System.Data;

namespace MSc.Cloud.Orchestration.EventsService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(IDbConnection dbConnection) : ControllerBase
{
    /// <summary>
    /// Get all events.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var reservations = dbConnection.Query<Event>(NamesValues.Queries.Events.GetEventsNotDeleted);
        return Ok(reservations);
    }

    /// <summary>
    /// Get an event by id.
    /// </summary>
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    /// <summary>
    /// Create new event.
    /// </summary>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    /// <summary>
    /// Mark an event as deleted.
    /// </summary>
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

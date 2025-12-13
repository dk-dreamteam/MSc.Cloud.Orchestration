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
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //var reservations = dbConnection.Query<Reservation>(NamesValues.Queries.Reservations.GetReservations);
        var reservations = dbConnection.Query<Event>(NamesValues.Queries.Events.GetEventsNotDeleted);
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

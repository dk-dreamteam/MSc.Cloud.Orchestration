using Dapper;
using Microsoft.AspNetCore.Mvc;
using MSc.Cloud.Orchestration.EventsService.Models;
using System.Data;

namespace MSc.Cloud.Orchestation.EventsService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(IDbConnection dbConnection) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var reservations = dbConnection.Query<Reservation>("SELECT * FROM \"Reservations\".\"Reservation\"");
        return Ok(reservations);
    }

    // GET api/<ValuesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ValuesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

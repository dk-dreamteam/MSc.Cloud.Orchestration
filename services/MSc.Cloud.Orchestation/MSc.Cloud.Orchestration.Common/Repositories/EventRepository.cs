using Dapper;
using MSc.Cloud.Orchestration.Common.Contracts;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using System.Data;
using static MSc.Cloud.Orchestration.Common.NamesValues.Queries.Events;

namespace MSc.Cloud.Orchestration.Common.Repositories;

public sealed class EventRepository(IDbConnection connection) : IEventRepository
{
    public async Task<int> CreateAsync(CreateEventRequest request) => await connection.ExecuteScalarAsync<int>(CreateEvent, request);

    public async Task<EventDto?> GetByIdAsync(int id) => await connection.QuerySingleOrDefaultAsync<EventDto>(GetEventById, new { Id = id });

    public async Task<IEnumerable<EventDto>> ListAsync() => await connection.QueryAsync<EventDto>(GetEvents);

    public async Task<bool> DeleteAsync(int id) => await connection.ExecuteScalarAsync<bool>(DeleteEvent, new { Id = id });
}
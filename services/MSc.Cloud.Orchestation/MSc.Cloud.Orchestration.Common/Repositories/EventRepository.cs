using Dapper;
using MSc.Cloud.Orchestration.Common.Contracts;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using System.Data;

namespace MSc.Cloud.Orchestration.Common.Repositories;

public sealed class EventRepository(IDbConnection connection) : IEventRepository
{
    public async Task<int> CreateAsync(CreateEventRequest request)
    {
        const string sql = """
            SELECT "Events".create_event(
                @Name,
                @Description,
                @StartsAt,
                @ImgUrl
            );
            """;

        return await connection.ExecuteScalarAsync<int>(sql, request);
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT *
            FROM "Events".get_event_by_id(@Id);
            """;

        return await connection.QuerySingleOrDefaultAsync<EventDto>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<EventDto>> ListAsync()
    {
        const string sql = """
            SELECT *
            FROM "Events".list_events();
            """;

        return await connection.QueryAsync<EventDto>(sql);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            SELECT "Events".delete_event(@Id);
            """;

        return await connection.ExecuteScalarAsync<bool>(
            sql,
            new { Id = id });
    }
}
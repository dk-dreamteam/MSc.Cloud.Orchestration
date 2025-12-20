using Dapper;
using MSc.Cloud.Orchestration.Common.Contracts;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using System.Data;

namespace MSc.Cloud.Orchestration.Common.Repositories;

public sealed class ReservationRepository : IReservationRepository
{
    private readonly IDbConnection _connection;

    public ReservationRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(CreateReservationRequest request)
    {
        const string sql = """
            SELECT "Reservations".create_reservation(
                @EventId,
                @FullName,
                @NumTickets,
                @EmailAddress
            );
            """;

        return await _connection.ExecuteScalarAsync<int>(sql, request);
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT *
            FROM "Reservations".get_reservation_by_id(@Id);
            """;

        return await _connection.QuerySingleOrDefaultAsync<ReservationDto>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<ReservationDto>> ListAsync()
    {
        const string sql = """
            SELECT *
            FROM "Reservations".list_reservations();
            """;

        return await _connection.QueryAsync<ReservationDto>(sql);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
            SELECT "Reservations".delete_reservation(@Id);
            """;

        return await _connection.ExecuteScalarAsync<bool>(
            sql,
            new { Id = id });
    }
}
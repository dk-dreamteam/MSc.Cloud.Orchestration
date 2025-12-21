using Dapper;
using MSc.Cloud.Orchestration.Common.Models;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using System.Data;
using static MSc.Cloud.Orchestration.Common.NamesValues.Queries.Reservations;

namespace MSc.Cloud.Orchestration.Common.Repositories;

public sealed class ReservationRepository(IDbConnection connection) : IReservationRepository
{
    public async Task<int> CreateAsync(CreateReservationRequest request) => await connection.ExecuteScalarAsync<int>(CreateReservation, request);

    public async Task<ReservationDto?> GetByIdAsync(int id) => await connection.QuerySingleOrDefaultAsync<ReservationDto>(GetReservationById, new { Id = id });

    public async Task<IEnumerable<ReservationDto>> ListAsync() => await connection.QueryAsync<ReservationDto>(GetReservations);

    public async Task<bool> DeleteAsync(int id) => await connection.ExecuteScalarAsync<bool>(DeleteReservation, new { Id = id });
}
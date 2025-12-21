using MSc.Cloud.Orchestration.Common.Models;

namespace MSc.Cloud.Orchestration.Common.Repositories.Interfaces;

public interface IReservationRepository
{
    Task<int> CreateAsync(CreateReservationRequest request);
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<IEnumerable<ReservationDto>> ListAsync();
    Task<bool> DeleteAsync(int id);
}

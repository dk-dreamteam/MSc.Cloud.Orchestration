using MSc.Cloud.Orchestration.Common.Models;

namespace MSc.Cloud.Orchestration.Common.Repositories.Interfaces;

public interface IEventRepository
{
    Task<int> CreateAsync(CreateEventRequest request);
    Task<EventDto?> GetByIdAsync(int id);
    Task<IEnumerable<EventDto>> ListAsync();
    Task<bool> DeleteAsync(int id);
}

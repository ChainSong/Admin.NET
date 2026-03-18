using TaskPlaApplication.Domain.Entities;
using TaskPlaApplication.Domain;

namespace TaskPlaApplication.Infrastructure.Persistence;
public interface ITaskRepository
{
    Task<List<WMSInstruction>> GetPendingAsync(int take, CancellationToken ct = default);
    Task UpdateAsync(WMSInstruction item, InstructionStatus statusCode, CancellationToken ct = default);
}

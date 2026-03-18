// Application/Contracts/IInstructionHandler.cs
using TaskPlaApplication.Domain;
using TaskPlaApplication.Domain.Entities;

namespace TaskPlaApplication.Application.Contracts;

public interface IInstructionHandler
{
    InstructionType Type { get; }
    Task<HandlerResult> HandleAsync(WMSInstruction instruction, string Token , CancellationToken ct);
}

public record HandlerResult(bool Success, string? Result = null, object? CallbackPayload = null);

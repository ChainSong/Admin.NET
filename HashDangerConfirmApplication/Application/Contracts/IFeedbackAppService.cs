using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlaApplication.Shared.Results;

namespace TaskPlaApplication.Application.Contracts
{
    public interface IFeedbackAppService
    {
        Task ProcessAsync(CancellationToken ct = default);
    }
}

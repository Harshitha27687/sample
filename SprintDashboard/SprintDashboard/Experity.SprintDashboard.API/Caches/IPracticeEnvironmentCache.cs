using System;
using System.Threading.Tasks;
using Experity.SprintDashboard.Models;

namespace Experity.SprintDashboard.API.Caches
{
    public interface IPracticeEnvironmentCache
    {
        Task<Practice> GetPracticeAsync(string practiceAbbrev);
        Task<Practice> GetPracticeByPkAsync(Guid practicePk);
    }
}
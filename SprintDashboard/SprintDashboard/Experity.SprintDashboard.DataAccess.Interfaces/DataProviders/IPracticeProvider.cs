using System.Collections.Generic;
using System.Threading.Tasks;
using Experity.SprintDashboard.Models;

namespace Experity.SprintDashboard.DataAccess.Interfaces.DataProviders
{
    public interface IPracticeProvider
    {
        Task<IEnumerable<Practice>> GetPracticesAsync();
    }
}

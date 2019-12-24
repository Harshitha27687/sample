using Experity.SprintDashboard.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Experity.SprintDashboard.DataAccess.Interfaces.DataProviders
{
    public interface ITeamProvider
    {
        Task<IEnumerable<TeamDto>> GetTeamsAsync();
    }
}

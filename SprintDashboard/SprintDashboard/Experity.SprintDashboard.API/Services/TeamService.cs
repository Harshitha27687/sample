using Experity.SprintDashboard.Data.DTOs;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experity.SprintDashboard.API.Services
{
    public class TeamService
    {
        private readonly ITeamProvider _teamProvider;

        public TeamService(ITeamProvider provider)
        {
            _teamProvider = provider;
        }

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync()
        {
            var teams = await _teamProvider.GetTeamsAsync();
            return teams;
        }
    }
}

using Dapper;
using Experity.SprintDashboard.Data.DTOs;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Experity.SprintDashboard.DataAccess.Dapper
{
    public class TeamProvider: ITeamProvider
    {
        private readonly string _connectionString;

        public TeamProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync()
        {
            var query = @"SELECT TeamId, TeamName, CreatedDate FROM Team ORDER BY TeamName";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<TeamDto>(query);
            }
        }
    }
}

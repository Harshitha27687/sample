using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Experity.SprintDashboard.Data.DTOs;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;

namespace Experity.SprintDashboard.DataAccess.Dapper
{
    public class ClinicProvider : IClinicProvider
    {
        private readonly string _connectionString;

        public ClinicProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ClinicDto>> GetClinicsAsync(Guid practicePk)
        {
            var query =
                @"SELECT c.ClinicPk, c.PracticePk, c.ClinicName
                FROM [SprintDashboard].[dbo].[Clinic] AS c
                WHERE c.PracticePk = @PracticePk
                ORDER BY c.Name";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<ClinicDto>(query,
                    new { PracticePk = practicePk });
            }
        }

    }
}

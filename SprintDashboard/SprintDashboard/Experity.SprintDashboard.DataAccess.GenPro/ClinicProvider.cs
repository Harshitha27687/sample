using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experity.SprintDashboard.Data.DTOs;
using Experity.SprintDashboard.Data.GenPro.Entities.EntityClasses;
using Experity.SprintDashboard.Data.GenPro.Entities.FactoryClasses;
using Experity.SprintDashboard.Data.GenPro.Entities.HelperClasses;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;

namespace Experity.SprintDashboard.DataAccess.GenPro
{
    public class ClinicProvider : IClinicProvider
    {
        private readonly string _environment;
        private readonly string _connectionString;

        public ClinicProvider(string environment, string connectionString)
        {
            _environment = environment;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ClinicDto>> GetClinicsAsync(Guid practicePk)
        {
            var qf = new QueryFactory();
            var q = qf.Create<ClinicEntity>()
                      .Where(ClinicFields.PracticePk.Equal(practicePk));

            using (var adapter = AdapterFactory.CreateAdapter(_connectionString, _environment))
            {
                var results = await adapter.FetchQueryAsync<ClinicEntity>(q);

                var clinics = new List<ClinicDto>();

                foreach (ClinicEntity clinic in results)
                {
                    clinics.Add(new ClinicDto()
                    {
                        ClinicName = clinic.Name,
                        ClinicPk = clinic.ClinicPk,
                        PracticePk = clinic.PracticePk
                    });
                }
                return clinics;
            }
        }
    }
}

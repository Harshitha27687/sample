using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experity.SprintDashboard.Data.DTOs;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;

namespace Experity.SprintDashboard.API.Services
{
    public class ClinicService
    {
        private readonly IClinicProvider _clinicProvider;

        public ClinicService(IClinicProvider provider)
        {
            _clinicProvider = provider;
        }

        public async Task<IEnumerable<ClinicDto>> GetClinicsAsync(Guid practicePk)
        {
            //This is a simple fetch so there isn't much logic here and this class looks like a needless passthrough.
            //However, The service class should be responsible for interacting with models and for now the dataproviders.
            var clinics = await _clinicProvider.GetClinicsAsync(practicePk);
            return clinics;
        }
    }
}

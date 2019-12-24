using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experity.SprintDashboard.Data.DTOs;

namespace Experity.SprintDashboard.DataAccess.Interfaces.DataProviders
{
    public interface IClinicProvider
    {
        Task<IEnumerable<ClinicDto>> GetClinicsAsync(Guid practicePk);
    }
}
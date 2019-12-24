using System;

namespace Experity.SprintDashboard.Data.DTOs
{
    public class ClinicDto
    {
        public Guid ClinicPk { get; set; }
        public Guid PracticePk { get; set; }

        public string ClinicName { get; set; }
    }
}

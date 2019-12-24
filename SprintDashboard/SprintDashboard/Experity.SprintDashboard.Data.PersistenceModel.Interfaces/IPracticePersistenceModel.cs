using System;

namespace Experity.SprintDashboard.Data.PersistenceModel.Interfaces
{
    public interface IPracticePersistenceModel
    {
        string Environment { get; set; }
        string Practice { get; set; }
        Guid PracticePk { get; set; }
    }
}

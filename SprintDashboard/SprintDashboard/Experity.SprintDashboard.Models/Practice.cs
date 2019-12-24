using System;
using Experity.SprintDashboard.Data.PersistenceModel.Interfaces;

namespace Experity.SprintDashboard.Models
{
    public class Practice
    {
        public string Environment { get; set; }
        public string Name { get; set; }
        public Guid PracticePk { get; set; }

        public Practice()
        {
        }

        public Practice(IPracticePersistenceModel practicePersistenceModel)
        {
            //model parameters should be validated so that the model is always in a valid state
            //we have not determined how to handle this yet.
            Environment = practicePersistenceModel.Environment;
            Name = practicePersistenceModel.Practice;
            PracticePk = practicePersistenceModel.PracticePk;
        }

        //Any behavior related to practice belongs in this class.  
    }
}

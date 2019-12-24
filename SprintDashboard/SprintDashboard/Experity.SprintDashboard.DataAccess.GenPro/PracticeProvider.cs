using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Experity.SprintDashboard.Data.GenPro.Entities.EntityClasses;
using Experity.SprintDashboard.Data.GenPro.Entities.FactoryClasses;
using Experity.SprintDashboard.Data.GenPro.Entities.HelperClasses;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using Experity.SprintDashboard.Models;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;

namespace Experity.SprintDashboard.DataAccess.GenPro
{
    using System.Linq;


    public class PracticeProvider : IPracticeProvider
    {
        private readonly string _connectionString;

        public PracticeProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Practice>> GetPracticesAsync()
        {
            var qf = new QueryFactory();
            var q = qf.Create<PracticeEntity>();

            using (var adapter = AdapterFactory.CreateAdapter(_connectionString, "DEVTEST"))
            {
                var practiceEntities = await adapter.FetchQueryAsync(q);

                var practices = new List<Practice>();
                foreach (PracticeEntity practice in practiceEntities)
                {
                    practices.Add(new Practice(practice));
                }

                return practices;
            }
        }
    }
}

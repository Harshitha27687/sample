using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Experity.SprintDashboard.DataAccess.Interfaces.DataProviders;
using Experity.SprintDashboard.Models;

namespace Experity.SprintDashboard.API.Caches
{
    public class PracticeEnvironmentCache : IPracticeEnvironmentCache
    {
        private IMemoryCache _cache { get; }
        private readonly ILogger<PracticeEnvironmentCache> _logger;
        private readonly IPracticeProvider _repo;

        public const string KeyName = "PracticeEnv";
        public PracticeEnvironmentCache(IMemoryCache memoryCache, IPracticeProvider repo, ILogger<PracticeEnvironmentCache> logger)
        {
            _repo = repo;
            _cache = memoryCache;
            _logger = logger;
        }

        private async Task PopulateCacheAsync()
        {
            _logger.LogInformation("Started Populating Cache");
            ClearCache();

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            var items = await _repo.GetPracticesAsync();
            _cache.Set(KeyName, items, cacheEntryOptions);

            _logger.LogInformation("Cache Populated");
        }

        public async Task<Practice> GetPracticeAsync(string practice)
        {
            var practices = await GetPracticesAsync();
            return practices.FirstOrDefault(x => x.Name.Equals(practice, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Practice> GetPracticeByPkAsync(Guid practicePk)
        {
            var practices = await GetPracticesAsync();
            return practices.FirstOrDefault(x => x.PracticePk.Equals(practicePk));
        }

        private async Task<IEnumerable<Practice>> GetPracticesAsync()
        {
            if (!_cache.TryGetValue(KeyName, out IEnumerable<Practice> practiceEnvironments))
            {
                await PopulateCacheAsync();
                _cache.TryGetValue(KeyName, out practiceEnvironments);
            }

            return practiceEnvironments;
        }

        private void ClearCache()
        {
            _logger.LogInformation("Started Clearing Cache");
            _cache.Remove(KeyName);
            _logger.LogInformation("Cache Cleared");
        }
    }
}

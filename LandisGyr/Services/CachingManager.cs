using LandisGyr.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LandisGyr.Tests")]

namespace LandisGyr.Services
{
    public class CachingManager
    {
        internal readonly IMemoryCache _memoryCache;

        internal CachingManager(IMemoryCache cacheProvider)
        {
            _memoryCache = cacheProvider;
        }

        //ToDo make exceptions
        //ToDo fix returns

        internal ICollection<ConsumerUnit> GetConsumerUnitsFromCache()
        {
            ICollection<ConsumerUnit>? consumerUnits = _memoryCache.Get<ICollection<ConsumerUnit>>("ConsumerUnits");
            return consumerUnits ?? new List<ConsumerUnit>();
        }

        internal ConsumerUnit? FindConsumerUnitFromCache(string serialNumber)
        {
            ICollection<ConsumerUnit> consumerUnits = GetConsumerUnitsFromCache();
            return consumerUnits.Any()
                ? consumerUnits.FirstOrDefault(c => c.SerialNumber == serialNumber)
                : throw new ArgumentNullException("Endpoint not found");
        }

        internal void AddConsumerUnitToCache(ConsumerUnit consumerUnit)
        {
            ICollection<ConsumerUnit> consumerUnits = GetConsumerUnitsFromCache();
            consumerUnits.Add(consumerUnit);
            _memoryCache.Set("ConsumerUnits", consumerUnits);
        }

        internal void UpdateConsumerUnitInCache(ConsumerUnit consumerUnit)
        {
            ICollection<ConsumerUnit> consumerUnits = GetConsumerUnitsFromCache();
            ConsumerUnit? existingConsumerUnit = consumerUnits.FirstOrDefault(c => c.SerialNumber == consumerUnit.SerialNumber);

            if (existingConsumerUnit != null)
            {
                consumerUnits.Remove(existingConsumerUnit);
            }
            consumerUnits.Add(consumerUnit);
            _memoryCache.Set("ConsumerUnits", consumerUnits);
        }

        internal void RemoveConsumerUnitFromCache(string serialNumber)
        {
            ICollection<ConsumerUnit> consumerUnits = GetConsumerUnitsFromCache();

            ConsumerUnit? consumerUnit = consumerUnits.FirstOrDefault(c => c.SerialNumber == serialNumber);

            if (consumerUnit != null)
            {
                consumerUnits.Remove(consumerUnit);
                _memoryCache.Set("ConsumerUnits", consumerUnits);
            }
        }
    }
}

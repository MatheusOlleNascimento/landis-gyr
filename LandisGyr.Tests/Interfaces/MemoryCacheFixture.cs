using Microsoft.Extensions.Caching.Memory;

namespace LandisGyr.Tests.Interfaces
{
    public class MemoryCacheFixture : IClassFixture<MemoryCacheFixture>
    {
        internal IMemoryCache Cache { get; private set; }

        public MemoryCacheFixture()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }
    }
}

using LandisGyr.Models;
using LandisGyr.Services;
using LandisGyr.Tests.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace LandisGyr.Tests.Services
{
    public class CachingManagerTests : IClassFixture<MemoryCacheFixture>
    {
        private readonly IMemoryCache _cache;

        public CachingManagerTests(MemoryCacheFixture fixture)
        {
            _cache = fixture.Cache;
        }

        [Fact]
        public void VerifyMemoryCacheEmpty()
        {
            // Arrange
            var cachingManager = new CachingManager(_cache);

            // Act
            var result = cachingManager.GetConsumerUnitsFromCache();

            // Assert
            Assert.Equal(new List<ConsumerUnit>(), result);
        }
    }
}
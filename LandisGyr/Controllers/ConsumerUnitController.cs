using LandisGyr.Enums;
using LandisGyr.Models;
using LandisGyr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LandisGyr.Controllers
{
    //ToDo fix returns, verify status in cache and return this
    //ToDo refactor code FindConsumerUnitFromCache logic duplicate a lot
    //ToDo create validate inputs
    //ToDo create Dtos

    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerUnitController : CachingManager
    {
        public ConsumerUnitController(IMemoryCache cacheProvider) : base(cacheProvider) { }

        [HttpPost("insert")]
        public bool CreateConsumerUnit(ConsumerUnit consumerUnit)
        {
            ConsumerUnit? consumerUnitFinded = FindConsumerUnitFromCache(consumerUnit.SerialNumber);
            if (consumerUnitFinded != null) throw new InvalidOperationException("This serial number is already in use");

            AddConsumerUnitToCache(consumerUnit);

            return true;
        }

        [HttpPut("edit")]
        public bool EditConsumerUnit(string serialNumber, SwitchState switchState) //ToDo create a DTO
        {
            ConsumerUnit? consumerUnit = FindConsumerUnitFromCache(serialNumber);
            if (consumerUnit == null) throw new ArgumentNullException("ConsumerUnit not found");

            consumerUnit.SwitchState = switchState;
            UpdateConsumerUnitInCache(consumerUnit);

            return true;
        }

        [HttpDelete("delete")]
        public bool DeleteConsumerUnit(string serialNumber)
        {
            ConsumerUnit? consumerUnit = FindConsumerUnitFromCache(serialNumber);
            if (consumerUnit == null) throw new ArgumentNullException("ConsumerUnit not found");

            RemoveConsumerUnitFromCache(serialNumber);

            return true;
        }

        [HttpGet("list")]
        public ICollection<ConsumerUnit> ListConsumerUnits() => GetConsumerUnitsFromCache();

        [HttpGet("find")]
        public ConsumerUnit FindConsumerUnit(string serialNumber)
        {
            ConsumerUnit? consumerUnit = FindConsumerUnitFromCache(serialNumber);

            return consumerUnit ?? throw new ArgumentNullException("ConsumerUnit not found");
        }
    }
}
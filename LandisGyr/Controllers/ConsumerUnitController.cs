using LandisGyr.Enums;
using LandisGyr.Models;
using LandisGyr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LandisGyr.Controllers
{
    //ToDo fix returns, verify status in cache and return this
    //ToDo refactor code FindConsumerUnitFromCache logic duplicate a lot

    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerUnitController : CachingManager
    {
        public ConsumerUnitController(IMemoryCache cacheProvider) : base(cacheProvider) { }

        // Solicitará que o usuário insira todos os atributos de um endpoint e o armazene
        // Se já existir "Número de série do endpoint", uma mensagem de erro deverá ser exibida
        [HttpPost("insert")]
        public bool CreateConsumerUnit(ConsumerUnit consumerUnit)
        {
            ConsumerUnit? consumerUnitFinded = FindConsumerUnitFromCache(consumerUnit.SerialNumber);
            if (consumerUnitFinded != null) throw new InvalidOperationException("This serial number is already in use");
            
            AddConsumerUnitToCache(consumerUnit);

            return true;
        }

        // Solicitará primeiro que o usuário insira um "Número de série do ponto final"
        // Localizará um endpoint com o "Endpoint Serial Number" fornecido(ou exibirá uma mensagem de erro se ele não for encontrado)
        // Dará ao usuário a opção de alterar apenas o "Estado do interruptor"
        [HttpPut("edit")]
        public bool EditConsumerUnit(string serialNumber, SwitchState switchState) //ToDo create a DTO
        {
            ConsumerUnit? consumerUnit = FindConsumerUnitFromCache(serialNumber);
            if (consumerUnit == null) throw new ArgumentNullException("ConsumerUnit not found");

            consumerUnit.SwitchState = switchState;
            UpdateConsumerUnitInCache(consumerUnit);

            return true;
        }

        // Solicitará primeiro que o usuário insira um "Número de série do ponto final"
        // Localizará um endpoint com o "Endpoint Serial Number" fornecido(ou exibirá uma mensagem de erro se ele não for encontrado)
        // Solicitará a confirmação do usuário e excluirá o endpoint
        [HttpDelete("delete")]
        public bool DeleteConsumerUnit(string serialNumber)
        {
            ConsumerUnit? consumerUnit = FindConsumerUnitFromCache(serialNumber);
            if (consumerUnit == null) throw new ArgumentNullException("ConsumerUnit not found");

            RemoveConsumerUnitFromCache(serialNumber);

            return true;
        }

        // Essa opção imprimirá na tela todos os detalhes de todos os endpoints
        [HttpGet("list")] 
        public ICollection<ConsumerUnit> ListConsumerUnits() => GetConsumerUnitsFromCache();

        // Solicitará primeiro que o usuário insira um "Número de série do ponto final"
        // Localizará o ponto final com o "Endpoint Serial Number" fornecido(ou fornecerá uma mensagem de erro se ele não for encontrado)
        // Imprimirá na tela todos os detalhes do ponto final
        [HttpGet("find")]
        public ConsumerUnit FindConsumerUnit(string serialNumber) {
            ConsumerUnit? consumerUnit = FindConsumerUnitFromCache(serialNumber);
            
            return consumerUnit ?? throw new ArgumentNullException("ConsumerUnit not found");
        }
    }
}
using LandisGyr.Enums;
using LandisGyr.Interfaces;
using LandisGyr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LandisGyr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EndpointController : CacheController
    {
        private readonly IMemoryCache _memoryCache;
        private List<ConsumerUnit> consumerUnits = new();

        public EndpointController(IMemoryCache cacheProvider)
        {
            _memoryCache = cacheProvider;
        }

        //I don't created dtos, bacause the models and input is equal

        // Solicitará que o usuário insira todos os atributos de um endpoint e o armazene
        // Se já existir "Número de série do endpoint", uma mensagem de erro deverá ser exibida
        [HttpPost("insert")]
        public bool CreateEndpoint(ConsumerUnit consumerUnit)
        {
            ConsumerUnit? consumerUnitFinded = consumerUnits.Find(c => c.SerialNumber == consumerUnit.SerialNumber);
            if (consumerUnitFinded != null) throw new InvalidOperationException("This serial number is already in use");

            consumerUnits.Add(consumerUnit);
            return consumerUnits.Any(c => c.SerialNumber == consumerUnit.SerialNumber);
        }

        // Solicitará primeiro que o usuário insira um "Número de série do ponto final"
        // Localizará um endpoint com o "Endpoint Serial Number" fornecido(ou exibirá uma mensagem de erro se ele não for encontrado)
        // Dará ao usuário a opção de alterar apenas o "Estado do interruptor"
        [HttpPut("edit")]
        public bool EditEndpoint(string serialNumber, SwitchState switchState) //ToDo create a DTO
        {
            ConsumerUnit consumerUnit = FindConsumerUnit(serialNumber);

            consumerUnits = consumerUnits.Select(c =>
            {
                if (c.SerialNumber == serialNumber) c.SwitchState = switchState;
                return c;
            }).ToList();

            return true;
        }

        // Solicitará primeiro que o usuário insira um "Número de série do ponto final"
        // Localizará um endpoint com o "Endpoint Serial Number" fornecido(ou exibirá uma mensagem de erro se ele não for encontrado)
        // Solicitará a confirmação do usuário e excluirá o endpoint
        [HttpDelete("delete")]
        public bool DeleteEndpoint(string serialNumber) 
        {
            ConsumerUnit consumerUnit = FindConsumerUnit(serialNumber);
            consumerUnits.Remove(consumerUnit);
            
            return !consumerUnits.Any(c => c.SerialNumber == serialNumber);
        }

        [HttpGet("list")] // Essa opção imprimirá na tela todos os detalhes de todos os endpoints
        public ICollection<ConsumerUnit> ListEndpoints() => consumerUnits;

        // Essa opção solicitará primeiro que o usuário insira um "Endpoint Serial Number" (número de série do ponto final)
        // Localizará o ponto final com o "EndpointSerial Number" fornecido(ou fornecerá uma mensagem de erro se ele não for encontrado)
        // Imprimirá na tela todos os detalhes do ponto final
        [HttpGet("find")]
        public ConsumerUnit FindEndpoint(string serialNumber) => FindConsumerUnit(serialNumber);

        private ConsumerUnit FindConsumerUnit(string serialNumber) {
            ConsumerUnit? consumerUnit = consumerUnits.Find(c => c.SerialNumber == serialNumber);
            if (consumerUnit != null) return consumerUnit;

            throw new ArgumentNullException("Endpoint not found");
        }
    }
}
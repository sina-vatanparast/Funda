using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.DataContract;
using Funda.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace Funda.Presentation
{
    [Route("api/Broker")]
    public class BrokerController : Controller
    {
        // Some parameters as constants here, but they can pass throught the APIs
        private const string City = "amsterdam";
        private const int TopCount = 10;

        private readonly IBrokerService _brokerService;

        public BrokerController(IBrokerService brokerService)
        {
            _brokerService = brokerService;
        }

        // EndPoint 1
        // GET: {baseUrl}/api/api/broker
        [HttpGet]
        public async Task<List<BrokerModel>> Get()
        {
            return await _brokerService.GetTopOnes(TopCount, City);
        }


        // EndPoint 2
        // GET: {baseUrl}/api/api/broker/garden
        [Route("Garden")]
        [HttpGet]
        public async Task<List<BrokerModel>> GetGarden()
        {
            return await _brokerService.GetTopOnes(TopCount, City, true);
        }
    }
}

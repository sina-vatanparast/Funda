using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.DataContract;
using Funda.ServiceContract;

namespace Funda.Service
{
    public  class BrokerService : IBrokerService
    {
        private readonly IObjectApiHttpClient _objectApiHttpClient;

        public BrokerService(IObjectApiHttpClient objectApiHttpClient)
        {
            _objectApiHttpClient = objectApiHttpClient;
        }
        /// <summary>
        /// Get all objects in the city with the condition (with gardern and without) and returns the top n brokers provided the most objects
        /// </summary>
        /// <param name="count">the number of top brokers</param>
        /// <param name="city">the name of the city</param>
        /// <param name="garden">indicates object with a garden or without</param>
        /// <returns>a list of top n brokers provided the most objects </returns>
        public async Task<List<BrokerModel>> GetTopOnes(int count, string city, bool garden = false)
        {
            var obejcts = await _objectApiHttpClient.Execute(city, garden);

            var groupedObjects =
                obejcts.GroupBy(broker => new { broker.MakelaarId, broker.MakelaarNaam })
                    .Select(group => new BrokerModel
                    {
                        BrokerId = group.Key.MakelaarId,
                        BrokerName = group.Key.MakelaarNaam,
                        ObjectCount = group.Count()
                    });

            return groupedObjects.OrderByDescending(p => p.ObjectCount).Take(count).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.DataContract;


namespace Funda.ServiceContract
{
    /// <summary>
    /// Get all objects in the city with the condition (with gardern and without) and returns the top n brokers provided the most objects
    /// </summary>
    /// <param name="count">the number of top brokers</param>
    /// <param name="city">the name of the city</param>
    /// <param name="garden">indicates object with a garden or without</param>
    /// <returns>a list of top n brokers provided the most objects </returns>
    public interface IBrokerService
    {
        Task<List<BrokerModel>> GetTopOnes(int count, string city, bool garden = false);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.DataContract;

namespace Funda.ServiceContract
{
    public interface IObjectApiHttpClient
    {
        Task<IReadOnlyCollection<ObjectModel>> Execute(string city, bool garden);
    }
}

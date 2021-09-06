using System.Collections.Generic;

namespace Funda.DataContract
{
    public class FundaApiResponseModel
    {
        public List<ObjectModel> Objects { get; set; }
        public PagingModel Paging { get; set; }
        public int TotaalAantalObjecten { get; set; }       //Total Number of Objects

    }
}

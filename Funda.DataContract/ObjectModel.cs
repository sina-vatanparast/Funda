using System;

namespace Funda.DataContract
{
    public class ObjectModel
    {
        public string MakelaarNaam { get; set; }    // Broker Name
        public long MakelaarId { get; set; }    //Broker Id
        public long GlobalId { get; set; }
        public Guid Id { get; set; }
    }
}

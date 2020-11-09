using System.Collections.Generic;

namespace SentinelForwarder.Domain
{
    public class SentinelList
    {
        public string Name { get; set; }
        public List<SentinelIncident> Value { get; set; }
    }
}
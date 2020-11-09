using System.Collections.Generic;

namespace SentinelForwarder.Domain
{
    public class SentinelIncidentAdditionalData
    {
        public int AlertsCount { get; set; }
        public int BookmarksCount { get; set; }
        public int CommentsCount { get; set; }
        public List<string> AlertProductNames { get; set; }
        public List<string> Tactics { get; set; }
    }
}
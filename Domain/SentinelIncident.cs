namespace SentinelForwarder.Domain
{
    public class SentinelIncident
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Etag { get; set; }
        public string Type { get; set; }

        public SentinelIncidentProperties Properties { get; set; }
    }
}
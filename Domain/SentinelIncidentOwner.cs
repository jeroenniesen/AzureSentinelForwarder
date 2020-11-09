namespace SentinelForwarder.Domain
{
    public class SentinelIncidentOwner
    {
        public string ObjectId { get; set; }
        public string Email { get; set; }
        public string AssignedTo { get; set; }
        public string UserPrincipalName { get; set; }
    }
}
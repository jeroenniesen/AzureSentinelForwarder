namespace SentinelForwarder.Domain
{
    public class AppRegistration
    {
        public int AppRegistrationId { get; set; }
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string DirectoryId { get; set; }
        public string AppSecret { get; set; }
    }
}
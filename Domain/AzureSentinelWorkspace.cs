namespace SentinelForwarder.Domain
{
    public class AzureSentinelWorkspace
    {
        /// <summary>
        /// The subscription in which the Azure Sentinel workspace is deployed
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// The Resource Group in which the Azure Sentinel workspace is deployed
        /// </summary>
        public string ResourceGroup { get; set; }

        /// <summary>
        /// The Log Analytics woskpace which is used by Azure Sentinel
        /// </summary>
        public string WorkspaceName { get; set; }

        /// <summary>
        /// The App Registration configuration which is used to authenticate to Azure Sentinel
        /// </summary>
        public AppRegistration AppRegistration { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public AzureSentinelWorkspace()
        {   
            AppRegistration = new AppRegistration();
        }

    }
}
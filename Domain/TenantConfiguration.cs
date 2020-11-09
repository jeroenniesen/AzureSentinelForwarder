using System.Collections.Generic;

namespace SentinelForwarder.Domain
{
    public class TenantConfiguration
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// The Azure Sentinel Workspaces which are used for this tenant
        /// </summary>
        public List<AzureSentinelWorkspace> AzureSentinelWorkspaces { get; set; }
    }
}
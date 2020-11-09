using System;
using System.Collections.Generic;

namespace SentinelForwarder.Domain
{
    public class SentinelIncidentProperties
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IncidentStatus Status { get; set; }
        public int IncidentNumber { get; set; }
        public string IncidentUrl { get; set; }
        public string Classification { get; set; }
        public string ClassificationComment { get; set; }
        public string ClassificationReason { get; set; }

        public SentinelIncidentOwner Owner { get; set; }
        public List<SentinelIncidentLabel> Labels { get; set; }

        public DateTime FristActivityTimeUtc { get; set; }
        public DateTime LastActivityTimeUtc { get; set; }
        public DateTime LastModifiedTimeUtc { get; set; }
        public DateTime CreatedTimeUtc { get; set; }

        public DateTime FistActivityTimeGenerated { get; set; }
        public DateTime LastActivityTimeGenerated { get; set; }

        public List<string> RelatedAnalyticRuleIds { get; set; }

        public IncidentSeverity Severity { get; set; }
    }
}
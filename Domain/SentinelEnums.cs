namespace SentinelForwarder.Domain
{
    public enum IncidentClassification
    {
        None,
        BenignPositive,
        FalsePositive,
        TruePositive,
        Undetermined
    }

    public enum IncidentClassificationReason
    {
        None,
        InaccurateData,
        IncorrectAlertLogic,
        SuspiciousActivity,
        SuspiciousButExpected
    }

    public enum IncidentLabelType
    {
        System,
        User
    }

    public enum IncidentSeverity
    {
        None,
        High,
        Informational,
        Low,
        Medium
    }

    public enum IncidentStatus
    {
        Active,
        Closed,
        New
    }    
}
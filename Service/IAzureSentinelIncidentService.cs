using SentinelForwarder.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SentinelForwarder.Service
{
    public interface IAzureSentinelIncidentService
    {
        Task<SentinelIncident> GetIncidentAsync(string IncidentId, AzureSentinelWorkspace Workspace);
        Task UpdateIncidentAsync(SentinelIncident Incident, string IncidentId, AzureSentinelWorkspace Workspace);
        Task DeleteIncidentAsync(string IncidentId, AzureSentinelWorkspace Workspace);
        Task<List<SentinelIncident>> GetIncidentsAsync(IncidentStatus status, int limit, AzureSentinelWorkspace Workspace);
    }
}
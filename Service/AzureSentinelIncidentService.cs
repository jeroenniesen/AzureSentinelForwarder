using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SentinelForwarder.Domain;

namespace SentinelForwarder.Service
{
    public class AzureSentinelIncidentService : AzureSentinelServiceBase, IAzureSentinelIncidentService
    {
        /// <summary>
        /// Delete an incident from an Azure Sentinel Workspace
        /// </summary>
        /// <param name="IncidentId">The ID of the incident to be deleted</param>
        /// <param name="Workspace">The Workspace of which the incident should be deleted</param>
        /// <returns></returns>
        public async Task DeleteIncidentAsync(string IncidentId, AzureSentinelWorkspace Workspace)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync(Workspace.AppRegistration.DirectoryId, Workspace.AppRegistration.ClientId, Workspace.AppRegistration.AppSecret);
 
            string url = String.Format("https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.OperationalInsights/workspaces/{2}/providers/Microsoft.SecurityInsights/incidents/{3}?api-version=2020-01-01",
                                Workspace.SubscriptionId,
                                Workspace.ResourceGroup,
                                Workspace.WorkspaceName,
                                IncidentId);
            

            var response = await httpClient.DeleteAsync(url);    
            var message = await response.Content.ReadAsStringAsync();

            if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                throw new ArgumentException(String.Format("Deleting incident {0} resulted in: 403 - Forbidden", IncidentId));
            }

            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                throw new ArgumentException(String.Format("Something went wrong: '{0}'", message));
            } 

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                throw new ArgumentException(String.Format("Incident {0} not found in workspace {1}", IncidentId, Workspace.WorkspaceName));
            }
        }

        /// <summary>
        /// Get an incident from an Azure Sentinel Workspace
        /// </summary>
        /// <param name="IncidentId">The ID of the incident</param>
        /// <param name="Workspace">The workspace in which the incident resides</param>
        /// <returns></returns>
        public async Task<SentinelIncident> GetIncidentAsync(string IncidentId, AzureSentinelWorkspace Workspace)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync(Workspace.AppRegistration.DirectoryId, Workspace.AppRegistration.ClientId, Workspace.AppRegistration.AppSecret);
            
            string url = String.Format("https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.OperationalInsights/workspaces/{2}/providers/Microsoft.SecurityInsights/incidents/{3}?api-version=2020-01-01",
                                Workspace.SubscriptionId,
                                Workspace.ResourceGroup,
                                Workspace.WorkspaceName,
                                IncidentId);

            var response = await httpClient.GetAsync(url);
            
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound) {
                throw new ArgumentException(String.Format("Incident {0} not found in workspace {1}", IncidentId, Workspace.WorkspaceName));
            }

            if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                throw new ArgumentException(String.Format("Getting Incident {0} resulted in: 403 - Forbidden", IncidentId));
            }

            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                var message = await response.Content.ReadAsStringAsync();
                throw new ArgumentException(String.Format("Something went wrong: '{0}'", message));
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var incident = JsonSerializer.Deserialize<SentinelIncident>(responseJson, GetJsonSerializerOptions()); 

            return incident;                   
        }

        /// <summary>
        /// Get incidents from an Azure Sentinel Workspace
        /// </summary>
        /// <param name="status">The status of the incident</param>
        /// <param name="limit">The amount of incidents to get</param>
        /// <param name="Workspace">The workspace in which the incidents reside</param>
        /// <returns></returns>
        public async Task<List<SentinelIncident>> GetIncidentsAsync(IncidentStatus status, int limit, AzureSentinelWorkspace Workspace)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync(Workspace.AppRegistration.DirectoryId, Workspace.AppRegistration.ClientId, Workspace.AppRegistration.AppSecret);
            
            string url = String.Format("https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.OperationalInsights/workspaces/{2}/providers/Microsoft.SecurityInsights/incidents?api-version=2020-01-01&$filter=properties/status eq '{3}'&$top={4}&$orderby=properties/createdTimeUtc desc",
                                Workspace.SubscriptionId,
                                Workspace.ResourceGroup,
                                Workspace.WorkspaceName,
                                status.ToString(),
                                limit);
            
            var response = await httpClient.GetAsync(url);

            if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                throw new ArgumentException(String.Format("Getting incidents from workspace {0} resulted in: 403 - Forbidden", Workspace.WorkspaceName));
            }

            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                var message = await response.Content.ReadAsStringAsync();
                throw new ArgumentException(String.Format("Something went wrong: '{0}'", message));
            }
            
            var responseJson = await response.Content.ReadAsStringAsync();
            var incidentsResult = JsonSerializer.Deserialize<SentinelList>(responseJson, GetJsonSerializerOptions());
            
            return incidentsResult.Value;
        }

        /// <summary>
        /// Update an incident in a Azure Sentinel Workspace
        /// </summary>
        /// <param name="Incident">The incident</param>
        /// <param name="IncidentId">The id of the incident</param>
        /// <param name="Workspace">The workspace in which the incident resides</param>
        /// <returns></returns>
        public async Task UpdateIncidentAsync(SentinelIncident Incident, string IncidentId, AzureSentinelWorkspace Workspace)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync(Workspace.AppRegistration.DirectoryId, Workspace.AppRegistration.ClientId, Workspace.AppRegistration.AppSecret);
 
            string url = String.Format("https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.OperationalInsights/workspaces/{2}/providers/Microsoft.SecurityInsights/incidents/{3}?api-version=2020-01-01",
                                Workspace.SubscriptionId,
                                Workspace.ResourceGroup,
                                Workspace.WorkspaceName,
                                IncidentId);
            
            var json = JsonSerializer.Serialize<SentinelIncident>(Incident, GetJsonSerializerOptions());
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(url, content);    
            var message = await response.Content.ReadAsStringAsync();

            if(response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
                throw new ArgumentException(String.Format("Updating incident {0} resulted in: 403 - Forbidden", IncidentId));
            }

            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                throw new ArgumentException(String.Format("Something went wrong: '{0}'", message));
            }           
        }
    }
}
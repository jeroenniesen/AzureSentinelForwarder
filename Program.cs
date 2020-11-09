using System;
using SentinelForwarder.Domain;
using SentinelForwarder.Service;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace SentinelForwarder
{
    class Program
    {   
        public static Timer _Timer { get; set; }
        public static AzureSentinelWorkspace _Workspace {get;set;}
        public static IEventHubService _EventHubService { get; set; }

        static async Task Main(string[] args)
        {
            _Workspace = new AzureSentinelWorkspace();
            _Workspace.SubscriptionId = Environment.GetEnvironmentVariable("AzureSubscriptionId");
            _Workspace.ResourceGroup = Environment.GetEnvironmentVariable("AzureSentinelResourceGroup"); 
            _Workspace.WorkspaceName = Environment.GetEnvironmentVariable("AzureSentinelWorkspaceName");
            _Workspace.AppRegistration.DirectoryId = Environment.GetEnvironmentVariable("AppRegistrationDirectoryId");
            _Workspace.AppRegistration.ClientId = Environment.GetEnvironmentVariable("AppRegistrationClientId");
            _Workspace.AppRegistration.AppSecret = Environment.GetEnvironmentVariable("AppRegistrationSecret");

            System.Console.WriteLine(String.Format("Azure Subscription Id: {0}", _Workspace.SubscriptionId));
            System.Console.WriteLine(String.Format("Resource Group: {0}", _Workspace.ResourceGroup));
            System.Console.WriteLine(String.Format("Workspace: {0}", _Workspace.WorkspaceName));
            System.Console.WriteLine(String.Format("App Registration ClientId: {0}", _Workspace.AppRegistration.ClientId));
            System.Console.WriteLine(String.Format("App Registration DirectoryId:: {0}", _Workspace.AppRegistration.DirectoryId));
            System.Console.WriteLine(String.Format("Eventhub Name: {0}", Environment.GetEnvironmentVariable("EventHubName")));

            _EventHubService = new EventHubService(Environment.GetEnvironmentVariable("EventHubConnectionString"), Environment.GetEnvironmentVariable("EventHubName"));

            while(true) {               
                await ProcessSentinelIncidentsAsync();
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        public static async Task ProcessSentinelIncidentsAsync() 
        {
            var sentinelIncidentService = new AzureSentinelIncidentService();
            var incidents = await sentinelIncidentService.GetIncidentsAsync(IncidentStatus.New, 20, _Workspace);

            foreach(var incident in incidents) 
            {
                var stringEnumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                options.Converters.Add(stringEnumConverter);

                var json = JsonSerializer.Serialize<SentinelIncident>(incident, options);

                await _EventHubService.PublishMessageAsync(json);

                // update the incident status.
                incident.Properties.Status = IncidentStatus.Active;

                await sentinelIncidentService.UpdateIncidentAsync(incident, incident.Name, _Workspace);
                
                System.Console.WriteLine(String.Format("Incidentnr {0} is forwared to eventhub", incident.Properties.IncidentNumber));
            }    
        }
    }
}

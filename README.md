# AzureSentinelForwarder
A simple container that will forward all your Azure Sentinel incidents to an Eventhub

Azure Sentinel has no out-of-the box push mechanism. By forwarding all your incidents to an eventhub, you can basically get that push mechanism by subscribing on the eventhub. You can accoumplish the forward by building a logic app (which you need de configure for each analytics rule that you create) or use this project. 

How does it work?
1. Azure Sentinel Forwarder periodicaly (once per 10 seconds) checks for incidents with the status 'New' in the Azure Sentinel Instance
2. Azure Sentinel Forwarder publishes the new incidents on the Azure EventHub
3. Azure Sentinel Forwarder updates the incident with the status 'Activce' so it won't be processed twice.
4. You can subscribe with your application or SIEM to the eventhub and receive all incidents with their raw data.

## How to get started
1. Create an App Registrationd and configure it with a secret. Make sure it has "Azure Sentinel Contributor" permissions on your Azure Subscription. For more info read: https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app. You will need the ClientId, DirectoryId and Secret later.
2. Create an EventHub namespace with an Eventhub in it. In the "Shared Access policies" tab of the Eventhub, create a new policy which has Send permissions. You will need the ConnectionString that is defined in this page later.
3. After cloning the project, edit the docker-compose.yaml and fill in:
   - AzureSubscriptionId
   - AzureSentinelResourceGroup
   - AzureSentinelWorkspaceName
   - AppRegistrationClientId
   - AppRegistrationDirectoryId
   - AppRegistrationSecret
   - EventHubConnectionString
   - EventHubName     
4. Make sure docker is installed. Run from a commandshell docker-compose up.
5. The AzureForwarder will now forward all events from your Azure Sentinel Workspace to an EventHub
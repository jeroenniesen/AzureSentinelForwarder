# AzureSentinelForwarder
A simple container that will forward all your Azure Sentinel incidents to an Eventhub

Azure Sentinel has no out-of-the box push mechanism. By forwarding all your incidents to an eventhub, you can basically get that push mechanism by subscribing on the eventhub. You can accoumplish the forward by building a logic app (which you need de configure for each analytics rule that you create) or use this project. 

How does it work?
1. Azure Sentinel Forwarder periodicaly (once per 10 seconds) checks for incidents with the status 'New' in the Azure Sentinel Instance
2. Azure Sentinel Forwarder publishes the new incidents on the Azure EventHub
3. Azure Sentinel Forwarder updates the incident with the status 'Activce' so it won't be processed twice.
4. You can subscribe with your application or SIEM to the eventhub and receive all incidents with their raw data.

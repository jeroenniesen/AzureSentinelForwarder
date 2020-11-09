using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace SentinelForwarder.Service
{
    public class EventHubService : IEventHubService
    {
        public string _ConnectionString { get; set; }
        public string _EventHubName { get; set; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        /// <param name="connectionString">The connectionstring of the Eventhub</param>
        /// <param name="eventhubName">The name of the eventhub</param>
        public EventHubService(string connectionString, string eventhubName)
        {
                _ConnectionString = connectionString;
                _EventHubName = eventhubName;
        }

        /// <summary>
        /// Publish a message to an EventHub
        /// </summary>
        /// <param name="JsonMessage"></param>
        public async Task PublishMessageAsync(string JsonMessage)
        {
            await using (var producerClient = new EventHubProducerClient(_ConnectionString, _EventHubName))
            {
                // Create a batch of events 
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonMessage)));

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
            }
        }
    }
}
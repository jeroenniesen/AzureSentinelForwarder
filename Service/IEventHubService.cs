using System.Threading.Tasks;

namespace SentinelForwarder.Service
{
    public interface IEventHubService
    {
        Task PublishMessageAsync(string JsonMessage);
    }
}
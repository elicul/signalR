using System.Threading.Tasks;

namespace SignalR.Contracts.Interfaces.Hubs
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, string payload);
    }
}

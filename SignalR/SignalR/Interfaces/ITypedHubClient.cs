using System.Threading.Tasks;

namespace SignalR.Interfaces
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, string payload);
    }
}

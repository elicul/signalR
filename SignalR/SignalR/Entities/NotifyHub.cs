using Microsoft.AspNetCore.SignalR;
using SignalR.Interfaces;
using System.Threading.Tasks;

namespace SignalR.Entities
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using SignalR.Interfaces;

namespace SignalR.Entities
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
    }
}

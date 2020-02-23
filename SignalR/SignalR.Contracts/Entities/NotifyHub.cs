using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts.Interfaces.Hubs;
using System;
using System.Threading.Tasks;

namespace SignalR.Contracts.Entities
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}

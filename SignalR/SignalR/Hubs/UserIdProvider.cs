using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.ConnectionId;
        }
    }
}

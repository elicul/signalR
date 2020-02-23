using Microsoft.AspNetCore.SignalR;

namespace SignalR.Contracts.Entities
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.ConnectionId;
        }
    }
}

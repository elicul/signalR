using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SignalR.Entities
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.ConnectionId;
        }
    }
}

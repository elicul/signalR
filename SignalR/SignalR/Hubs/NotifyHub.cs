using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts.DTOs;
using SignalR.Contracts.Interfaces.Domain;
using SignalR.Contracts.Interfaces.Hubs;
using System;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
        private readonly IUserService userService;

        public NotifyHub(IUserService userService)
        {
            this.userService = userService;
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task<ResultDto> SaveUserConnection(UserDto user)
        {
            return await userService.SaveUserAsync(user);
        }
    }
}

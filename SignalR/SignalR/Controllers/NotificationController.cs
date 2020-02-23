using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Domain;
using SignalR.Contracts.Interfaces.Hubs;

namespace SignalR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : BaseController
    {
        private readonly IUserService _userService;
        private IHubContext<NotifyHub, ITypedHubClient> _hubContext;

        public NotificationController(IHubContext<NotifyHub, ITypedHubClient> hubContext, IUserService userService)
        {
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]Message msg)
        {
            string retMessage;

            try
            {
                _hubContext.Clients.Client(msg.ConnectionId).BroadcastMessage(msg.Type, msg.Payload);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }
    }
}
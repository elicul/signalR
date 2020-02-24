using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Domain;
using SignalR.Contracts.Interfaces.Hubs;
using SignalR.Hubs;

namespace SignalR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : BaseController
    {
        private readonly IUserService userService;
        private IHubContext<NotifyHub, ITypedHubClient> hubContext;

        public NotificationController(IHubContext<NotifyHub, ITypedHubClient> hubContext, IUserService userService)
        {
            this.userService = userService;
            this.hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]Message msg)
        {
            string retMessage;

            try
            {
                hubContext.Clients.Client(msg.ConnectionId).BroadcastMessage(msg.Type, msg.Payload);
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
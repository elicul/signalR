using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalR.Contracts.Entities;
using SignalR.Contracts.Interfaces.Domain;

namespace SignalR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : BaseController
    {
        private readonly IUserService userService;

        public NotificationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("{tenantGuid}")]
        public async Task<ActionResult<string>> Post(Guid tenantGuid, [FromBody]Message message)
        {
            var result = await userService.SendMessageToTenantUsersAsync(tenantGuid, message);
            return GetApiResponseFromResultDto<string>(result);
        }

        [HttpPost("{email}/{tenantGuid}")]
        public async Task<ActionResult<string>> Post(string email, Guid tenantGuid, [FromBody]Message message)
        {
            var result = await userService.SendMessageToUserAsync(email, tenantGuid, message);
            return GetApiResponseFromResultDto<string>(result);
        }
    }
}
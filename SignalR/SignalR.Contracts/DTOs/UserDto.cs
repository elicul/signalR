using System;

namespace SignalR.Contracts.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public Guid TenantGuid { get; set; }
        public string TenantType { get; set; }
        public string ConnectionId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SignalR.Contracts.Entities
{
    public class User: BaseEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid TenantGuid { get; set; }
        public string TenantType { get; set; }
        public string ConnectionId { get; set; }
    }
}

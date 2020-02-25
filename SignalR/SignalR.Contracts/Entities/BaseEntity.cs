using System;

namespace SignalR.Contracts.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}

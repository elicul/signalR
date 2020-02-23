using System;

namespace SignalR.Contracts.Entities
{
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }

        public bool IsDeleted { get; set; }
    }
}

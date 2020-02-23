namespace SignalR.Contracts.Entities
{
    public class Message: BaseEntity
    {
        public string Type { get; set; }
        public string Payload { get; set; }
        public string ConnectionId { get; set; }
    }
}

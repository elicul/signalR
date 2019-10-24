namespace SignalR.Entities
{
    public class Message
    {
        public string Type { get; set; }
        public string Payload { get; set; }
        public string ConnectionId { get; set; }
    }
}

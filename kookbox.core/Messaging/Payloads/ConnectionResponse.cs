namespace kookbox.core.Messaging.Payloads
{
    [RegisteredPayload(MessageTypes.ConnectionResponse, 1)]
    public class ConnectionResponse : MessagePayload
    {
        public ConnectionResponse() 
        {
        }
    }
}

using kookbox.core.Messaging.DTO;

namespace kookbox.core.Messaging.Payloads
{
    [RegisteredPayload(MessageTypes.ConnectionResponse, 1)]
    public class ConnectionResponse : MessagePayload
    {
        public ConnectionResponse(RoomInfo activeRoom)
        {
            ActiveRoom = activeRoom;
        }

        public RoomInfo ActiveRoom { get; }
    }
}

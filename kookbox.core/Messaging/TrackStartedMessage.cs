using System;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging
{
    [RegisteredPayload(MessageTypes.TrackStarted, version: 1)]
    public class TrackStartedMessage : NetworkMessage
    {
        // todo: grab the runtime props from the attribute to avoid code
        public TrackStartedMessage(IMusicRoom room, IMusicTrack track)
            : base(MessageTypes.TrackStarted, 1)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            TrackId = track.Id;
            RoomId = room.Id;
        }

        public string RoomId { get; }
        public string TrackId { get; }
    }
}

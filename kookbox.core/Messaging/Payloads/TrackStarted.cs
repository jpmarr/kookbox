using System;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.Payloads
{
    [RegisteredPayload(MessageTypes.TrackStarted, version: 1)]
    public class TrackStarted : MessagePayload
    {
        public TrackStarted(IRoom room, ITrack track)
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

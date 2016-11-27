using System;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging
{
    public class TrackStartedMessage : NetworkMessage
    {
        public TrackStartedMessage(IMusicTrack track)
            : base(MessageTypes.TrackStarted, 1)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            TrackId = track.Id;
        }

        public string TrackId { get; }
    }
}

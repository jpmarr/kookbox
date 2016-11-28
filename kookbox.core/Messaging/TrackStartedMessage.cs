using System;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging
{
    [RegisteredPayload(MessageTypes.TrackStarted, version: 1)]
    public class TrackStartedMessage : NetworkMessage
    {
        // todo: grab the runtime props from the attribute to avoid code
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

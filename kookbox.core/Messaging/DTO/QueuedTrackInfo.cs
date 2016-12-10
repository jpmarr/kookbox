using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging.DTO
{
    // todo: consider using Option types here and writing a custom serializer for JSON to convert to nulls?
    public class QueuedTrackInfo
    {
        public QueuedTrackInfo(TrackInfo track, DateTimeOffset queuedTimestamp, ListenerInfo requester)
        {
            Track = track;
            QueuedTimestamp = queuedTimestamp;
            Requester = requester;
        }

        public static QueuedTrackInfo FromQueuedTrack(IQueuedTrack queued)
        {
            if (queued == null)
                return null;

            return new QueuedTrackInfo(
                TrackInfo.FromTrack(queued.Track),
                queued.QueuedTimestamp,
                ListenerInfo.FromListener(queued.Requester.ValueOr(null)));
        }

        public TrackInfo Track { get; }
        public DateTimeOffset QueuedTimestamp { get; }
        public ListenerInfo Requester { get; }
    }
}

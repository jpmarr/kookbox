using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class TrackQueue : ITrackQueue
    {
        private readonly List<IQueuedTrack> tracks = new List<IQueuedTrack>();

        public IEnumerable<IQueuedTrack> Tracks => tracks;
        public int Count => tracks.Count;

        public void QueueTrack(ITrack track)
        {
            tracks.Add(new QueuedTrack(track));
        }

        public Option<IQueuedTrack> DequeueNextTrack()
        {
            if (tracks.Count > 0)
            {
                var track = tracks.First();
                tracks.RemoveAt(0);
                return Option.Some(track);
            }
            return Option<IQueuedTrack>.None();
        }
    }
}

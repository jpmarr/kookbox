using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class MusicQueue : IMusicQueue
    {
        private readonly List<IQueuedMusicTrack> tracks = new List<IQueuedMusicTrack>();

        public IEnumerable<IQueuedMusicTrack> Tracks => tracks;
        public int Count => tracks.Count;

        public void QueueTrack(IMusicTrack track)
        {
            tracks.Add(new QueuedMusicTrack(track));
        }

        public Option<IQueuedMusicTrack> DequeueNextTrack()
        {
            if (tracks.Count > 0)
            {
                var track = tracks.First();
                tracks.RemoveAt(0);
                return Option.Some(track);
            }
            return Option<IQueuedMusicTrack>.None();
        }
    }
}

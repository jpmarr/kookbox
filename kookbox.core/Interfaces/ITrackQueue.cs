using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface ITrackQueue
    {
        IEnumerable<IQueuedTrack> Tracks { get; }
        int Count { get; }

        void QueueTrack(ITrack track);
        Option<IQueuedTrack> DequeueNextTrack();
    }
}

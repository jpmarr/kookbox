using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicQueue
    {
        IEnumerable<IQueuedMusicTrack> Tracks { get; }
        int Count { get; }

        void QueueTrack(IMusicTrack track);
        Option<IQueuedMusicTrack> DequeueNextTrack();
    }
}

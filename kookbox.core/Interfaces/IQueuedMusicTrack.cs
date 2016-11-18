using System;

namespace kookbox.core.Interfaces
{
    public interface IQueuedMusicTrack
    {
        IMusicTrack Track { get; }
        DateTimeOffset QueuedTimestamp { get; }
        Option<IPoll> Poll { get; }
        Option<IMusicListener> Requester { get; }
        Option<IMusicDedication> Dedication { get; }
    }
}

using System;

namespace kookbox.core.Interfaces
{
    public interface IQueuedMusicTrack
    {
        string Id { get; }
        int Position { get; }
        IMusicTrack Track { get; }
        DateTimeOffset QueuedTimestamp { get; }
        Option<IPoll> Poll { get; }
        Option<IMusicListener> Requester { get; }
        Option<IMusicDedication> Dedication { get; }
    }
}

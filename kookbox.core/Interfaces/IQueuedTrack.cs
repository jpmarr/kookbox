using System;

namespace kookbox.core.Interfaces
{
    public interface IQueuedTrack
    {
        string Id { get; }
        int Position { get; }
        ITrack Track { get; }
        DateTimeOffset QueuedTimestamp { get; }
        Option<IPoll> Poll { get; }
        Option<IUser> Requester { get; }
        Option<IDedication> Dedication { get; }
    }
}

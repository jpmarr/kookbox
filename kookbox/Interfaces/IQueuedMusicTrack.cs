using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
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

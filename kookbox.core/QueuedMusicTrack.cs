using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class QueuedMusicTrack : IQueuedMusicTrack
    {
        public QueuedMusicTrack(IMusicTrack track)
        {
            Track = track;
            QueuedTimestamp = DateTimeOffset.Now;
        }

        public IMusicTrack Track { get; }
        public DateTimeOffset QueuedTimestamp { get; }
        public Option<IPoll> Poll { get; }
        public Option<IMusicListener> Requester { get; }
        public Option<IMusicDedication> Dedication { get; }
    }
}

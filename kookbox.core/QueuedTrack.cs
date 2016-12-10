using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class QueuedTrack : IQueuedTrack
    {
        public QueuedTrack(ITrack track)
        {
            Track = track;
            QueuedTimestamp = DateTimeOffset.Now;
        }

        public string Id { get; }
        public int Position { get; }
        public ITrack Track { get; }
        public DateTimeOffset QueuedTimestamp { get; }
        public Option<IPoll> Poll { get; }
        public Option<IUser> Requester { get; }
        public Option<IDedication> Dedication { get; }
    }
}

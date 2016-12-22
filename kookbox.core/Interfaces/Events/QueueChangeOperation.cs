using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Events
{
    public class QueueChangeOperation
    {
        public QueueChangeType ChangeType { get; }
        public string TargetId { get; }
        public int Position;
        public Option<IQueuedTrack> New { get; }
    }

    public enum QueueChangeType
    {
        Add,
        Remove,
        Insert
    }
}

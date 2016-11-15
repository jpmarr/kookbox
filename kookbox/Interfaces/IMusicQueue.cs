using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicQueue
    {
        IEnumerable<IQueuedMusicTrack> Tracks { get; }
    }
}

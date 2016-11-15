using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicServer
    {
        IEnumerable<IMusicSource> Sources { get; }
        IEnumerable<IMusicListener> Listeners { get; }
        IEnumerable<IMusicRoom> Rooms { get; }
    }
}

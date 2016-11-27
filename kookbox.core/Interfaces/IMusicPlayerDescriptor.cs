using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicPlayerDescriptor
    {
        string Id { get; }
        string Description { get; }
        IMusicSource Source { get; }
        Option<IMusicRoom> CurrentRoom { get; }
    }
}

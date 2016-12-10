using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IPlayerDescriptor
    {
        string Id { get; }
        string Description { get; }
        IMusicSource Source { get; }
        Option<IRoom> CurrentRoom { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface INetworkMessage
    {
        short MessageType { get; }
        byte Version { get; }
        long CorrelationId { get; }
    }
}

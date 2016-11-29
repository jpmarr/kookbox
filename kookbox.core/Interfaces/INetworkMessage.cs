using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Messaging;

namespace kookbox.core.Interfaces
{
    public interface INetworkMessage
    {
        short MessageType { get; }
        byte Version { get; }
        long CorrelationId { get; }
    }

    public interface INetworkMessage<out T> : INetworkMessage where T : MessagePayload
    {
        T Payload { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Messaging.Payloads;

namespace kookbox.core.Messaging
{
    public static class MessageFactory
    {
        public static INetworkMessage ConnectionResponse()
        {
            return NetworkMessage.Create(new ConnectionResponse());
        }

        // todo: consider pools for these message classes...
        public static INetworkMessage TrackStarted(IMusicRoom room, IMusicTrack track)
        {
            return NetworkMessage.Create(new TrackStarted(room, track));
        }

        public static INetworkMessage Create(short messageType, byte version, long? correlationId, object payload)
        {
            return null;
        }
    }
}

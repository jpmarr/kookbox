using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging
{
    public static class MessageFactory
    {
        public static INetworkMessage ConnectionResponse()
        {
            return new ConnectionResponseMessage();
        }

        // todo: consider pools for these message classes...
        public static INetworkMessage TrackStarted(IMusicRoom room, IMusicTrack track)
        {
            return new TrackStartedMessage(room, track);
        }
    }
}

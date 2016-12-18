using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Messaging
{
    public static class MessageTypes
    {
        public const short TransportRunning = 0;
        public const short ConnectionResponse = 1;
        public const short TrackStarted = 2;
    }
}

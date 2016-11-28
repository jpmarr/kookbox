using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Messaging
{
    [RegisteredPayload(MessageTypes.ConnectionResponse, 1)]
    public class ConnectionResponseMessage : NetworkMessage
    {
        public ConnectionResponseMessage() 
            : base(MessageTypes.ConnectionResponse, 1)
        {
        }
    }
}

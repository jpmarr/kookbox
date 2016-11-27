using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Messaging
{
    public static class MessageRegistry
    {
        private static readonly Dictionary<short, Type> payloadTypes = new Dictionary<short, Type>();

        public static void RegisterPayloadType(short messageType, Type payloadType)
        {
            payloadTypes.Add(messageType, payloadType);   
        }

        public static bool TryGetPayloadType(short messageType, out Type payloadType)
        {
            // todo: report missing key
            return payloadTypes.TryGetValue(messageType, out payloadType);
        }
    }
}

using System.Threading;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging
{
    public abstract class NetworkMessage : INetworkMessage
    {
        private static long correlationId;

        protected NetworkMessage(short messageType, byte version)
            : this(messageType, version, Interlocked.Increment(ref correlationId))
        {
            
        }

        protected NetworkMessage(short messageType, byte version, long correlationId)
        {
            MessageType = messageType;
            Version = version;
            CorrelationId = correlationId;
        }

        public short MessageType { get; }
        public byte Version { get; }
        public long CorrelationId { get; }
    }
}

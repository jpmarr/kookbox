using System;
using System.Reflection;
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

        public static INetworkMessage Create<T>(T payload) where T: MessagePayload
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            // todo: need a cache of this/emit constructor delegates
            var attr = payload.GetType().GetTypeInfo().GetCustomAttribute<RegisteredPayloadAttribute>();

            return new NetworkMessage<T>(attr.MessageType, attr.Version, payload);
        }
    }

    public class NetworkMessage<T> : NetworkMessage,
        INetworkMessage<T> where T: MessagePayload
    {
        public NetworkMessage(short messageType, byte version, T payload) : base(messageType, version)
        {
            Payload = payload;
        }

        public NetworkMessage(short messageType, byte version, long correlationId, T payload) : base(messageType, version, correlationId)
        {
            Payload = payload;
        }

        public T Payload { get; }
    }
}

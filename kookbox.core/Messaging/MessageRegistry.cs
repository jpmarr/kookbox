using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using kookbox.core.Interfaces;

namespace kookbox.core.Messaging
{
    public static class MessageRegistry
    {
        private static readonly Dictionary<uint, Type> payloadTypes = new Dictionary<uint, Type>();

        public static void RegisterPayloadTypesInAssembly(Assembly implementingAssembly)
        {
            if (implementingAssembly == null)
                throw new ArgumentNullException(nameof(implementingAssembly));

            var registrations = 
                from t in implementingAssembly.GetTypes()
                let ti = t.GetTypeInfo()
                where ti.IsSubclassOf(typeof(NetworkMessage))
                let attr = ti.GetCustomAttribute<RegisteredPayloadAttribute>()
                where attr != null
                select new {attr.MessageType, attr.Version, PayloadType = t};

            foreach (var registration in registrations)
                RegisterPayloadType(registration.MessageType, registration.Version, registration.PayloadType);

        }

        public static void RegisterPayloadType(short messageType, byte version, Type payloadType)
        {
            var key = GetMessageKey(messageType, version);
            payloadTypes.Add(key, payloadType);
        }

        public static Type GetPayloadType(short messageType, byte version)
        {
            Type payloadType;
            if (!payloadTypes.TryGetValue(GetMessageKey(messageType, version), out payloadType))
                throw new ArgumentException($"Unable to located a registered payload type for messsage type {messageType} (v{version})");

            return payloadType;
        }

        internal static uint GetMessageKey(short messageType, byte version)
        {
            return ((uint)messageType << 16) + version;
        }
    }
}

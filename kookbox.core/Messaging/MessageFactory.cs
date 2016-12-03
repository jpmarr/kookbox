using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using kookbox.core.Interfaces;
using kookbox.core.Messaging.DTO;
using kookbox.core.Messaging.Payloads;

namespace kookbox.core.Messaging
{
    using MessageCreator = Func<long?, MessagePayload, INetworkMessage>;

    public static class MessageFactory
    {
        private static readonly Dictionary<uint, PayloadRegistration> payloadsByKey = new Dictionary<uint, PayloadRegistration>();
        private static readonly Dictionary<Type, PayloadRegistration> payloadsByType = new Dictionary<Type, PayloadRegistration>();

        public static INetworkMessage ConnectionResponse(RoomInfo activeRoom)
        {
            return NetworkMessage.Create(new ConnectionResponse(activeRoom));
        }

        // todo: consider pools for these message classes...
        public static INetworkMessage TrackStarted(IMusicRoom room, IMusicTrack track)
        {
            return NetworkMessage.Create(new TrackStarted(room, track));
        }

        public static PayloadRegistration GetPayloadRegistration(short messageType, byte version)
        {
            PayloadRegistration registration;
            if (!payloadsByKey.TryGetValue(GetPayloadKey(messageType, version), out registration))
                throw new ArgumentException($"Unable to located a registered payload type for messsage type {messageType} (v{version})");
            return registration;
        }

        public static void RegisterPayloadTypesInAssembly(Assembly implementingAssembly)
        {
            if (implementingAssembly == null)
                throw new ArgumentNullException(nameof(implementingAssembly));

            var registrations =
                from t in implementingAssembly.GetTypes()
                let ti = t.GetTypeInfo()
                where ti.IsSubclassOf(typeof(MessagePayload))
                let attr = ti.GetCustomAttribute<RegisteredPayloadAttribute>()
                where attr != null
                select new { attr.MessageType, attr.Version, PayloadType = t };

            foreach (var registration in registrations)
                RegisterPayloadType(registration.MessageType, registration.Version, registration.PayloadType);
        }

        public static void RegisterPayloadType(short messageType, byte version, Type payloadType)
        {
            var registration = new PayloadRegistration(messageType, version, payloadType);
            payloadsByKey.Add(GetPayloadKey(messageType, version), registration);
            payloadsByType.Add(payloadType, registration);
        }

        private static uint GetPayloadKey(short messageType, byte version)
        {
            return ((uint)messageType << 16) + version;
        }

        public class PayloadRegistration
        {
            private readonly MessageCreator creator;

            public PayloadRegistration(short messageType, byte version, Type payloadType)
            {
                MessageType = messageType;
                Version = version;
                PayloadType = payloadType;
                creator = GenerateCreator(payloadType);
            }

            public short MessageType { get; }
            public short Version { get; }
            public Type PayloadType { get; }

            public INetworkMessage CreateMessage(long? correlationId, MessagePayload payload)
            {
                return creator(correlationId, payload);
            }

            private static Func<long?, MessagePayload, INetworkMessage> GenerateCreator(Type payloadType)
            {
                var method = new DynamicMethod(
                    string.Empty,
                    typeof(INetworkMessage),
                    new[] { typeof(long?), typeof(MessagePayload) },
                    CodeGenerationContext.DynamicModule,
                    true);

                var messageType = typeof(NetworkMessage<>).MakeGenericType(payloadType);
                var ctor =
                    messageType.GetTypeInfo()
                        .GetConstructor(new[] { typeof(short), typeof(byte), typeof(long?), payloadType });
                var attr = payloadType.GetTypeInfo().GetCustomAttribute<RegisteredPayloadAttribute>();
                var codeGen = method.GetILGenerator();
                codeGen.Emit(OpCodes.Ldc_I4, (int)attr.MessageType); // push messageType onto stack as int32
                codeGen.Emit(OpCodes.Ldc_I4, (int)attr.Version);     // push version onto stack as int32
                codeGen.Emit(OpCodes.Ldarg_0);                       // push arg 0(correlationId) onto stack
                codeGen.Emit(OpCodes.Ldarg_1);                       // push arg 1(payload) ont stack
                codeGen.Emit(OpCodes.Castclass, payloadType);        // cast payload from base type to specific type
                codeGen.Emit(OpCodes.Newobj, ctor);                  // call NetworkMessage<T> constructor
                codeGen.Emit(OpCodes.Ret);                           // return

                return (Func<long?, MessagePayload, INetworkMessage>)method.CreateDelegate(
                    typeof(Func<long?, MessagePayload, INetworkMessage>));
            }

            private static class CodeGenerationContext
            {
                static CodeGenerationContext()
                {
                    var builder = AssemblyBuilder.DefineDynamicAssembly(
                        new AssemblyName("kookbox.core.Emit"),
                        AssemblyBuilderAccess.Run);
                    DynamicModule = builder.DefineDynamicModule("EmitModule");
                }

                public static ModuleBuilder DynamicModule { get; }
            }
        }
    }
}

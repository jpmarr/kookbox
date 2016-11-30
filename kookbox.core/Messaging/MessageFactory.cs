using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Messaging.Payloads;

namespace kookbox.core.Messaging
{
    using MessageCreator = Func<long?, MessagePayload, INetworkMessage>;

    public static class MessageFactory
    {
        private static readonly Dictionary<uint, MessageCreator> messageCreators = new Dictionary<uint, MessageCreator>();

        public static INetworkMessage ConnectionResponse()
        {
            return NetworkMessage.Create(new ConnectionResponse());
        }

        // todo: consider pools for these message classes...
        public static INetworkMessage TrackStarted(IMusicRoom room, IMusicTrack track)
        {
            return NetworkMessage.Create(new TrackStarted(room, track));
        }

        public static INetworkMessage Create(short messageType, byte version, long? correlationId, MessagePayload payload)
        {
            var key = MessageRegistry.GetMessageKey(messageType, version);
            MessageCreator creator;
            if (!messageCreators.TryGetValue(key, out creator))
            {
                creator = GenerateCreator(payload.GetType());
                messageCreators.Add(key, creator);
            }

            return creator(correlationId, payload);
        }

        private static Func<long?, MessagePayload, INetworkMessage> GenerateCreator(Type payloadType)
        {
            var method = new DynamicMethod(
                string.Empty,
                typeof(INetworkMessage),
                new[] { typeof(long?), payloadType },
                CodeGenerationContext.DynamicModule,
                true);

            var messageType = typeof(NetworkMessage<>).MakeGenericType(payloadType);
            var attr = payloadType.GetTypeInfo().GetCustomAttribute<RegisteredPayloadAttribute>();

            var codeGen = method.GetILGenerator();
            codeGen.Emit(OpCodes.Ldc_I4, attr.MessageType);
            codeGen.Emit(OpCodes.Ldc_I4, attr.Version);
            codeGen.Emit(OpCodes.Ldarg_0);
            codeGen.Emit(OpCodes.Ldarg_1);
            codeGen.Emit(OpCodes.Newobj, messageType.GetTypeInfo().GetConstructor(new[] { typeof(short), typeof(byte), typeof(long?), payloadType }));
            codeGen.Emit(OpCodes.Castclass, typeof(INetworkMessage));
            codeGen.Emit(OpCodes.Ret);

            var creator = method.CreateDelegate(typeof(Func<long?, MessagePayload, INetworkMessage>));
            return (Func<long?, MessagePayload, INetworkMessage>)creator;
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

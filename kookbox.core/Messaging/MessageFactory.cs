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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using kookbox.core.Interfaces;
using kookbox.core.Messaging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace kookbox.http
{
    internal class WebsocketNetworkTransport : INetworkTransport
    {
        private readonly HttpContext context;
        private WebSocket socket;
        private readonly Subject<INetworkMessage> messageSink = new Subject<INetworkMessage>();
        // todo: better buffer management
        private readonly byte[] writeBuffer = new byte[4096];
        private readonly byte[] readBuffer = new byte[4096];
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private readonly JsonSerializer serializer = JsonSerializer.CreateDefault(
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
        private readonly ActionBlock<INetworkMessage> messageQueue;

        public WebsocketNetworkTransport(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.context = context;
            messageQueue = new ActionBlock<INetworkMessage>(SendMessageAsync);
        }

        public async Task OpenAsync()
        {
            socket = await context.WebSockets.AcceptWebSocketAsync();
            QueueMessage(MessageFactory.ConnectionResponse());
            await BeginReceive();
        }

        public void QueueMessage(INetworkMessage message)
        {
            messageQueue.Post(message);
        }

        public IObservable<INetworkMessage> ReceivedMessages => messageSink;

        private async Task SendMessageAsync(INetworkMessage message)
        {
            int payloadLength;
            // todo: reuse stream and writer??
            using (var buffer = new MemoryStream(writeBuffer))
            using (var writer = new StreamWriter(buffer, Encoding.UTF8))
            {
                serializer.Serialize(writer, message);
                writer.Flush();
                payloadLength = (int)buffer.Position;
            }

            await socket.SendAsync(
                new ArraySegment<byte>(writeBuffer, 0, payloadLength),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        private async Task BeginReceive()
        {
            var offset = 0;
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(readBuffer, offset, readBuffer.Length - offset),
                    CancellationToken.None);

                if (result.EndOfMessage)
                {
                    var messageText = Encoding.UTF8.GetString(readBuffer, 0, offset + result.Count);
                    INetworkMessage message;
                    if (TryParseNetworkMessage(messageText, out message))
                        messageSink.OnNext(message);
                    else
                        throw new InvalidOperationException("Unable to parse message type");

                    offset = 0;
                }
                else
                    offset += result.Count;
            }

            messageSink.OnCompleted();
        }

        private bool TryParseNetworkMessage(string messageText, out INetworkMessage message)
        {
            /* Let's assume message structure is:
             * {
             *      messagetype: short,
             *      version: byte,
             *      payload: {
             *      
             *          serialised form of message payload type
             *      } 
             */
            short messageType = 0;
            byte version = 0;
            long? correlationId = null;
            JObject payloadJson = null;
            message = null;

            using (var reader = new JsonTextReader(new StringReader(messageText)) {CloseInput = true})
            {
                while (reader.Read())
                {
                    if (reader.Value == null || reader.TokenType != JsonToken.PropertyName)
                        continue;

                    var propertyName = reader.Value.ToString();
                    switch (propertyName)
                    {
                        case "messageType":
                            messageType = (short)(reader.ReadAsInt32() ?? 0);
                            if (messageType == 0)
                                return false;
                            break;
                        case "version":
                            version = (byte)(reader.ReadAsInt32() ?? 0);
                            if (version == 0)
                                return false;
                            break;
                        case "correlationId":
                            var correlationAsDecimal = reader.ReadAsDecimal();
                            if (correlationAsDecimal.HasValue)
                                correlationId = (long)correlationAsDecimal.Value;
                            break;
                        case "payload":
                            reader.Read();
                            payloadJson = JObject.Load(reader);
                            break;
                        default:
                            return false;
                    }
                }
            }

            if (messageType == 0 || version == 0 || payloadJson == null)
                return false;

            // todo: combine 2 hash lookups into 1 here
            message = MessageFactory.Create(
                messageType, 
                version, 
                correlationId,
                (MessagePayload)serializer.Deserialize(
                    payloadJson.CreateReader(),
                    MessageRegistry.GetPayloadType(messageType, version)));

            return true;
        }
    }
}

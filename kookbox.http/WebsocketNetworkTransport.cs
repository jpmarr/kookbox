using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using kookbox.core.Interfaces;
using kookbox.core.Messaging;
using Newtonsoft.Json;

namespace kookbox.http
{
    internal class WebsocketNetworkTransport : INetworkTransport
    {
        private readonly WebSocket socket;
        private readonly Subject<INetworkMessage> messageSink = new Subject<INetworkMessage>();
        // todo: better buffer management
        private readonly byte[] writeBuffer = new byte[4096];
        private readonly byte[] readBuffer = new byte[4096];
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private readonly JsonSerializer serializer = JsonSerializer.CreateDefault();

        public WebsocketNetworkTransport(WebSocket socket)
        {
            this.socket = socket;
            BeginReceive();
        }

        public Task SendMessageAsync(INetworkMessage message)
        {
            var payload = JsonConvert.SerializeObject(message);
            var payloadLength = Encoding.UTF8.GetBytes(payload, 0, payload.Length, writeBuffer, 0);

            return socket.SendAsync(
                new ArraySegment<byte>(writeBuffer, 0, payloadLength), 
                WebSocketMessageType.Text, 
                true,
                CancellationToken.None);
        }

        public IObservable<INetworkMessage> ReceivedMessages => messageSink;

        private async Task BeginReceive()
        {
            var offset = 0;
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(readBuffer, offset, readBuffer.Length - offset), 
                    cancellation.Token);

                if (result.EndOfMessage)
                {
                    var payload = Encoding.UTF8.GetString(readBuffer, 0, offset + result.Count);
                    using (var reader = new JsonTextReader(new StringReader(payload)) { CloseInput = true })
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

                        var messageType = reader.ReadAsInt32();
                        Type payloadType;
                        if (messageType == null || !MessageRegistry.TryGetPayloadType((short)messageType, 1, out payloadType))
                            throw new InvalidOperationException($"Unable to map payload type for message type: {messageType}");

                        var message = serializer.Deserialize(reader, payloadType) as INetworkMessage;
                        if (message == null)
                            throw new InvalidOperationException($"Payload type {payloadType.Name} is not a valid type.");

                        messageSink.OnNext(message);
                    }

                    offset = 0;
                }
                else
                    offset += result.Count;
            }

            messageSink.OnCompleted();
        }
    }
}

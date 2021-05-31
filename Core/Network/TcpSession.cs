using Proto;
using Service;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Core.Network
{
    public class TcpSession : MessageCoding, ISession
    {
        private byte[] buffer = new byte[1024];
        private Socket socket;

        public TcpSession()
        {
            
        }

        public void Dispose()
        {
            socket.Disconnect(false);
            
        }

        Action<INetMessage> onMsg;
        public void Init(Socket socket,Action<INetMessage> onMsg)
        {
            this.socket = socket;
            this.onMsg = onMsg;
        }

        public async void StartReceive(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var count = await socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
                if (count == 0)
                {
                    return;
                }
                else
                {
                    this.Write(buffer,0,count);
                }
            }
        }

        public async void SendMessage(INetMessage data)
        {
            await socket.SendAsync(ProroHelp.Encoder(data), SocketFlags.None);
        }

        public override void OnComplete(ushort msgid, byte[] data)
        {
            var msg = ProroHelp.Decoder(msgid, data);
            Log.Print(JsonSerializer.Serialize(msg as LoginRequest));
            onMsg?.Invoke(msg);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace Core.Network
{
    public class TcpSession: ISession
    {
        private TcpClient client;
        private NetworkStream networkStream;
        private byte[] sizebuffer = new byte[4];
        private Action<byte[]> messageDataHander;

        public void Initialize(TcpClient client,Action<byte[]> messageDataHander)
        {
            if (client == null)
            {
                throw new ArgumentNullException($"{nameof(client)} is null !");
            }
            if (messageDataHander == null)
            {
                throw new ArgumentNullException($"{nameof(messageDataHander)} is null !");
            }
            sizebuffer = new byte[4];
            this.client = client;
            this.messageDataHander = messageDataHander;

            networkStream = client.GetStream();
        }

        public async void StartReceive(CancellationToken cancellationToken)
        {
            while (true)
            {
                sizebuffer = await ReceiveBytes(sizebuffer, cancellationToken);
                if (sizebuffer == null || cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                else
                {
                    var messageData = await ReceiveBytes(new byte[BitConverter.ToInt32(sizebuffer, 0)], cancellationToken);
                    if (messageData == null || cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    else
                    {
                        messageDataHander.Invoke(messageData);
                    }
                }
            }
        }

        private int index;
        private int count;
        private async Task<byte[]> ReceiveBytes(byte[] buffer, CancellationToken cancellationToken)
        {
            do
            {
                try
                {
                    count = await networkStream.ReadAsync(buffer, index, buffer.Length - index, cancellationToken);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    return null;
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("断开 Tcp 接收");
                    return null;
                }
                if (count == 0)
                {
                    Debug.Log("Tcp 连接断开");
                    return null;
                }
            } while (index == buffer.Length);
            index = 0;
            return buffer;
        }

        private Queue<byte[]> sendQueue = new Queue<byte[]>(2 >> 5);
        public void SendMessage(byte[] data)
        {
            var newdata = new byte[data.Length + 4];
            Array.Copy(BitConverter.GetBytes(newdata.Length), newdata,4);
            Array.Copy(data, 0, newdata, 4, data.Length);

            sendQueue.Enqueue(newdata);
            if (sendQueue.Count == 1)
            {
                SendMessageData();
            }
        }

        private async void SendMessageData()
        {
            while (sendQueue.Count != 0)
            {
                var data = sendQueue.Dequeue();
                try
                {
                    await networkStream.WriteAsync(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    return;
                }
            }
        }

        public void Close()
        {
            client.Close();
            sendQueue.Clear();
            index = 0;
            count = 0;
            client = null;
        }
    }
}

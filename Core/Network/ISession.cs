using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Core.Client;

namespace Core.Network
{
    public interface ISession : IDisposable
    {
        public void Init(Socket socket);
        public void StartReceive(CancellationToken cancellationToken);
        public void SendMessage(byte[] data);
        public void Close();
    }

    public interface ICoding
    {
        void Write(byte[] data, int index, int length);
        void OnComplete(ushort msgid, byte[] data);
    }

    public class MessageCoding
    {

        public MessageCoding()
        {
            sizeArray = new byte[2];
            idArray = new byte[2];
        }

        private int count;
        private int msgLnegth = -1;
        private ushort id;
        private byte[] sizeArray;
        private byte[] idArray;
        private byte[] dataArray;

        public void Write(byte[] data, int index, int length)
        {
            int l = 0;
            while (length>0)
            {
                if (count < 2)
                {
                    l = 2 - count;
                    l = l > length ? length : l;
                    Array.Copy(data, index, sizeArray, count, l);
                }
                else if(count<4)
                {
                    l = 4 - count;
                    l = l > length ? length : l;
                    Array.Copy(data, index, idArray, count - 2, l);
                }
                else
                {
                    l = (4 + dataArray.Length) - count;
                    l = l > length ? length : l;
                    Array.Copy(data, index, dataArray, count - 4, l);
                }
                length -= l;
                index += l;
                count += l;
                if (count == 4)
                {
                    msgLnegth = BitConverter.ToInt16(sizeArray, 0);
                    id = BitConverter.ToUInt16(idArray);
                    dataArray = new byte[msgLnegth-4];
                }

                if (msgLnegth == count)
                {
                    Debug.Log("完整消息");
                    msgLnegth = -1;
                    count = 0;
                    l = 0;
                }
            }
        }
    }

    public class TcpSession : ISession
    {
        //private Memory<byte> memory = new Memory<byte>(new byte[1024]);
        private byte[] buffer = new byte[1024];
        private Socket socket;
        private byte[] sizeBytes = new byte[sizeof(ushort)];

        public void Dispose()
        {
            socket.Disconnect(false);
        }

        public void Init(Socket socket)
        {
            this.socket = socket;
        }

        public async void StartReceive(CancellationToken cancellationToken = default)
        {
            var count = await socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
        }

        public void SendMessage(byte[] data)
        {
            socket.SendAsync(new ReadOnlyMemory<byte>(data), SocketFlags.None);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
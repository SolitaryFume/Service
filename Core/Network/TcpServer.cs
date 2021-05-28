using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Core.Network
{
    public class TcpServer: ITcpServer
    {
        private const int backlog = 10;
        private Socket socket;

        private TcpServer()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Listen(backlog);
        }

        public void Start(string host, int port)
        {
            IPAddress address = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(address, port);
            socket.Bind(ipe);
            AcceptAsync();
        }

        public async void AcceptAsync()
        {
            while (true)
            {
                Socket client = await socket.AcceptAsync();
                OnAccept(client);
            }
        }

        public void OnAccept(Socket socket)
        {
            
        }
    }

    //public class TcpServer: ISession
    //{
    //    private TcpClient client;
    //    private NetworkStream networkStream;
    //    //private byte[] sizebuffer = new byte[4];
    //    private Memory<byte> buffer = new Memory<byte>(new byte[1024]);
    //    private Action<byte[]> messageDataHander;

    //    public void Initialize(TcpClient client,Action<byte[]> messageDataHander)
    //    {
    //        if (client == null)
    //        {
    //            throw new ArgumentNullException($"{nameof(client)} is null !");
    //        }
    //        if (messageDataHander == null)
    //        {
    //            throw new ArgumentNullException($"{nameof(messageDataHander)} is null !");
    //        }
    //        //sizebuffer = new byte[4];
    //        this.client = client;
    //        this.messageDataHander = messageDataHander;

    //        networkStream = client.GetStream();
    //        client.Client.ReceiveAsync(new Memory<byte>(new byte[1024]), SocketFlags.None);
    //    }

    //    public async void StartReceive(CancellationToken cancellationToken = default)
    //    {
    //        //while (true)
    //        //{
    //        //    var c = await client.Client.ReceiveAsync(new Memory<byte>(new byte[1024]), SocketFlags.None);
    //        //    sizebuffer = await ReceiveBytes(sizebuffer, cancellationToken);
    //        //    if (sizebuffer == null || cancellationToken.IsCancellationRequested)
    //        //    {
    //        //        return;
    //        //    }
    //        //    else
    //        //    {
    //        //        var messageData = await ReceiveBytes(new byte[BitConverter.ToInt32(sizebuffer, 0)], cancellationToken);
    //        //        if (messageData == null || cancellationToken.IsCancellationRequested)
    //        //        {
    //        //            return;
    //        //        }
    //        //        else
    //        //        {
    //        //            messageDataHander.Invoke(messageData);
    //        //        }
    //        //    }
    //        //}

    //        while (!cancellationToken.IsCancellationRequested)
    //        {
    //            var count = await client.Client.ReceiveAsync(buffer, SocketFlags.None,cancellationToken);
    //        }
    //    }

    //    private int index;
    //    private int count;
    //    private async Task<byte[]> ReceiveBytes(byte[] buffer, CancellationToken cancellationToken)
    //    {
    //        do
    //        {
    //            try
    //            {
    //                count = await networkStream.ReadAsync(buffer, index, buffer.Length - index, cancellationToken);
    //            }
    //            catch (Exception ex)
    //            {
    //                Debug.Log(ex);
    //                return null;
    //            }
    //            if (cancellationToken.IsCancellationRequested)
    //            {
    //                Debug.Log("断开 Tcp 接收");
    //                return null;
    //            }
    //            if (count == 0)
    //            {
    //                Debug.Log("Tcp 连接断开");
    //                return null;
    //            }
    //        } while (index == buffer.Length);
    //        index = 0;
    //        return buffer;
    //    }

    //    private Queue<byte[]> sendQueue = new Queue<byte[]>(2 >> 5);
    //    public void SendMessage(byte[] data)
    //    {
    //        var newdata = new byte[data.Length + 4];
    //        Array.Copy(BitConverter.GetBytes(newdata.Length), newdata,4);
    //        Array.Copy(data, 0, newdata, 4, data.Length);

    //        sendQueue.Enqueue(newdata);
    //        if (sendQueue.Count == 1)
    //        {
    //            SendMessageData();
    //        }
    //    }

    //    private async void SendMessageData()
    //    {
    //        while (sendQueue.Count != 0)
    //        {
    //            var data = sendQueue.Dequeue();
    //            try
    //            {
    //                await networkStream.WriteAsync(data, 0, data.Length);
    //            }
    //            catch (Exception ex)
    //            {
    //                Debug.Log(ex);
    //                return;
    //            }
    //        }
    //    }

    //    public void Close()
    //    {
    //        client.Close();
    //        sendQueue.Clear();
    //        index = 0;
    //        count = 0;
    //        client = null;
    //    }
    //}
}

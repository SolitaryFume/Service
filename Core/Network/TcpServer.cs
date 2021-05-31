using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Core.Client;

namespace Core.Network
{
    public class TcpServer: ITcpServer
    {
        private const int backlog = 10;
        private Socket socket;

        public TcpServer()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   
        }

        public void Start(string host, int port)
        {
            IPAddress address = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(address, port);
            socket.Bind(ipe);
            socket.Listen(backlog);
            AcceptAsync();
            Log.Print("网络监听模块启动!");
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
            var session = IOC.Root.GetService<ISession>();
            var proxy = IOC.Root.GetService<IClient>();
            proxy.Init(session);
            session.Init(socket, proxy.OnMessage);
            session.StartReceive();
        }
    }
}

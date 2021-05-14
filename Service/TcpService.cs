using System.Net;
using System.Net.Sockets;
using Core;

namespace Service
{
    public class TcpService : ServiceBase
    {
        private TcpListener _tcpListener;
        private TcpService()
        {
            // _tcpListener = new TcpListener();
        }

        public void Start(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
            Debug.Log("启用TcpListener!");
            Start();
        }

        public async void Start()
        {
            var tcp = await _tcpListener.AcceptSocketAsync();
            Debug.Log($"new tcp connect : {tcp.RemoteEndPoint}");
            var tokenSer = Services.GetService<TokenService>();
            var token = tokenSer.GetToken();
            token.Init(tcp);
            tokenSer.Register(token);
            Start();
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }
    }
}
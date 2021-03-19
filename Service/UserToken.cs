using System;
using System.Net.Sockets;

namespace Service
{
    public class UserToken : IToken
    {
        private Guid _guid;
        private Socket _socket;
        Guid IToken.ID { get => _guid;set=> _guid = value; }
        Socket IToken.Socket { get=>_socket; set=>_socket = value; }
        

        public void Dispose()
        {
            _guid = Guid.Empty;
            _socket?.Dispose();
            _socket = null;
        }

        public void Init(Socket socket)
        {
            _guid = Guid.NewGuid();
            _socket = socket;
            
            StartReceive();
        }

    
        public async void StartReceive()
        {
            var count = await _socket.ReceiveAsync(readMemory,SocketFlags.None);
            var msg = System.Text.Encoding.UTF8.GetString(readMemory,0,count);
            Debug.Log($"{this._socket.RemoteEndPoint}>>>{msg}");
            StartReceive();
        }

        public async void Send(string msg)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(msg);
            var count = await _socket.SendAsync(data,SocketFlags.None);
        }

        private byte[] readMemory = new byte[1024];
    }
}